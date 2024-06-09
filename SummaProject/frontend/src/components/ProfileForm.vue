<template>
    <div>
      <h1>{{ user ? "Update User" : "Create User" }}</h1>
      <form @submit.prevent="handleSubmit">
        <v-text-field v-model="formUser.username" label="Username" required></v-text-field>
        <v-text-field 
          v-model="formUser.email" 
          label="Email" 
          :disabled="user !== null"
          :error-messages="emailErrorMessage" 
          required>
        </v-text-field>
        <v-text-field 
          v-model="formUser.birthDate" 
          label="Date of Birth" 
          type="date" 
          :max="today"
          :error-messages="birthDateErrorMessage"
          required>
        </v-text-field>
        <v-file-input label="Photo" @change="handleFileChange"></v-file-input>
        <v-btn class="me-4" type="submit" :disabled="!isFormValid">{{ user ? "Update User" : "Create User" }}</v-btn>
        <v-btn @click="handleReset">Clear</v-btn>
      </form>
      <p v-if="message">{{ message }}</p>
    </div>
  </template>
  
  <script>
  import { createUpdateUser } from './userService';
  
  export default {
    props: {
      user: {
        type: Object,
        default: null,
      },
    },
    data() {
      return {
        formUser: this.user
          ? { ...this.user, photo: this.user.photo }
          : { username: '', email: '', birthDate: '', photo: null },
        message: '',
        today: new Date().toISOString().split('T')[0], // Get today's date in YYYY-MM-DD format
      };
    },
    computed: {
      emailErrorMessage() {
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return this.formUser.email && !emailPattern.test(this.formUser.email)
          ? 'Please enter a valid email address'
          : '';
      },
      birthDateErrorMessage() {
        return this.formUser.birthDate && this.formUser.birthDate > this.today
          ? 'Birth date cannot be a future date'
          : '';
      },
      isFormValid() {
        return (
          this.formUser.username &&
          this.formUser.email &&
          this.formUser.birthDate &&
          !this.emailErrorMessage &&
          !this.birthDateErrorMessage
        );
      },
    },
    methods: {
      async handleSubmit() {
        if (!this.isFormValid) {
          this.message = 'Please fill in all required fields correctly.';
          return;
        }
  
        try {
          const formData = new FormData();
          formData.append('Username', this.formUser.username);
          formData.append('Email', this.formUser.email);
          formData.append('BirthDate', this.formUser.birthDate);
  
          if (this.formUser.photo instanceof File) {
            formData.append('photo', this.formUser.photo);
          } else if (this.user && this.user.photo) {
            // Convert the existing photo (byte array or base64) to Blob
            const byteCharacters = atob(this.user.photo);
            const byteNumbers = new Array(byteCharacters.length).fill(0).map((_, i) => byteCharacters.charCodeAt(i));
            const byteArray = new Uint8Array(byteNumbers);
            const blob = new Blob([byteArray], { type: 'image/jpeg' });
            formData.append('photo', blob);
          }
  
          const response = await createUpdateUser(formData);
          this.message = `User created successfully with ID: ${response.id}`;
          this.handleReset();
          alert(`User created/updated successfully with ID: ${response.id}`);
          this.$emit('user-updated'); // Emit an event to notify parent component
        } catch (error) {
          this.message = `Error creating/updating user: ${error.message}`;
          alert(`Error creating/updating user: ${error.message}`);
        }
      },
      handleFileChange(event) {
        this.formUser.photo = event.target.files[0];
      },
      handleReset() {
        this.formUser = this.user
          ? { ...this.user, photo: this.user.photo }
          : { username: '', email: '', birthDate: '', photo: null };
        this.message = '';
      },
    },
    watch: {
      user(newUser) {
        if (newUser) {
          this.formUser = { ...newUser, photo: newUser.photo };
        }
      },
    },
  };
  </script>
  