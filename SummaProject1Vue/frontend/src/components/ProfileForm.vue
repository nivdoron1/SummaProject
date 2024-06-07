<template>
    <div>
        <h1>Create User</h1>
        <form @submit.prevent="handleSubmit">
            <div>
                <label for="username">Username:</label>
                <input type="text" v-model="user.username" required>
            </div>
            <div>
                <label for="email">Email:</label>
                <input type="email" v-model="user.email" required>
            </div>
            <div>
                <label for="dob">Date of Birth:</label>
                <input type="date" v-model="user.birthDate" required>
            </div>
            <div>
                <label for="photo">Photo:</label>
                <input type="file" @change="handleFileChange">
            </div>
            <button type="submit">Create User</button>
        </form>
        <p v-if="message">{{ message }}</p>
    </div>
</template>

<script>
import { createUser } from './userService';

export default {
    data() {
        return {
            user: {
                username: '',
                email: '',
                birthDate: '',
                photo: null // Store the file directly
            },
            message: ''
        };
    },
    methods: {
        async handleSubmit() {
            try {
                // Create a FormData object to handle file upload
                const formData = new FormData();
                formData.append('Username', this.user.username);
                formData.append('Email', this.user.email);
                formData.append('BirthDate', this.user.birthDate);
                if (this.user.photo) {
                    formData.append('photo', this.user.photo);
                }
                
                const newUser = await createUser(formData);
                this.message = `User created successfully with ID: ${newUser.id}`;
            } catch (error) {
                this.message = 'Error creating user';
            }
        },
        handleFileChange(event) {
            this.user.photo = event.target.files[0]; // Store the file object directly
        }
    }
};
</script>
