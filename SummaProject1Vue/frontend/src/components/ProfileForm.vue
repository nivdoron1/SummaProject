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

<script src="./userComponent.js"></script>
