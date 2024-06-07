<!-- UserList.vue -->
<template>
  <v-container>
    <h1>User List</h1>
    <v-list>
      <v-list-item
        v-for="(username, id) in users"
        :key="id"
        @click="showUserDetails(id)"
      >
        <v-list-item-content>
          <v-list-item-title>{{ username }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>

    <div v-if="selectedUser">
      <h2>User Details</h2>
      <p><strong>Username:</strong> {{ selectedUser.username }}</p>
      <p><strong>Email:</strong> {{ selectedUser.email }}</p>
      <p><strong>Birth Date:</strong> {{ selectedUser.birthDate }}</p>
    </div>
  </v-container>
</template>

<script>
import { getAllUsers, getUserById } from './userService';

export default {
  data() {
    return {
      users: {},
      selectedUser: null
    };
  },
  created() {
    this.fetchUsers();
  },
  methods: {
    async fetchUsers() {
      try {
        this.users = await getAllUsers();
      } catch (error) {
        console.error('Error fetching users:', error);
      }
    },
    async showUserDetails(userId) {
      try {
        this.selectedUser = await getUserById(userId);
      } catch (error) {
        console.error('Error fetching user details:', error);
      }
    }
  }
};
</script>

<style>
  .v-container {
    max-width: 600px;
    margin: 0 auto;
  }
</style>
