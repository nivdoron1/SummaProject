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
                <input type="date" v-model="user.dob" required>
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
                    dob: '',
                    photo: ''
                },
                message: ''
            };
        },
        methods: {
            async handleSubmit() {
                try {
                    const newUser = await createUser(this.user);
                    this.message = `User created successfully with ID: ${newUser.id}`;
                } catch (error) {
                    this.message = 'Error creating user';
                }
            },
            handleFileChange(event) {
                const file = event.target.files[0];
                const reader = new FileReader();
                reader.onload = (e) => {
                    this.user.photo = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        }
    };
</script>
