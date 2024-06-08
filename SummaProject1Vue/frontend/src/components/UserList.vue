<template>
  <v-container>
    <h1>User List</h1>
    <!-- Search Field -->
    <v-text-field
      v-model="searchQuery"
      label="Search Users"
      prepend-icon="mdi-magnify"
      clearable
    ></v-text-field>
    <!-- User List -->
    <v-list class="scrollable-list">
      <v-list-item v-for="(username, id) in filteredUsers" :key="id" @click="openUserDetails(id)">
        <v-list-item-content>
          <v-list-item-title>{{ username }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
    <!-- User Details Dialog -->
    <v-dialog v-model="showUserDetailsDialog" max-width="500">
      <v-card v-if="selectedUser">
        <v-img class="align-end text-white" height="200" :src="getImageUrl(selectedUser.photo)" cover></v-img>
        <v-card-text>
          <h2>User Details</h2>
          <p><strong>Username:</strong> {{ selectedUser.username }}</p>
          <p><strong>Email:</strong> {{ selectedUser.email }}</p>
          <p><strong>Birth Date:</strong> {{ selectedUser.birthDate }}</p>
        </v-card-text>
        <v-card-actions>
          <v-btn color="orange" @click="removeUser(selectedUser.id)" text>
            DELETE
          </v-btn>
          <v-btn color="orange" @click="openEditDialog" text>
            UPDATE
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeUserDetailsDialog">Close</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Edit User Dialog -->
    <v-dialog v-model="showEditDialog" max-width="500">
      <v-card>
        <v-card-title>
          <span class="headline">Update User</span>
        </v-card-title>
        <v-card-text>
          <ProfileForm v-if="selectedUser" :user="selectedUser" @user-updated="handleUpdate" />
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeEditDialog">Close</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import { getAllUsers, getUserById, deleteUserById } from './userService';
import ProfileForm from './ProfileForm.vue';

export default {
  components: {
    ProfileForm
  },
  data() {
    return {
      users: {},
      selectedUser: null,
      showEditDialog: false,
      showUserDetailsDialog: false,
      searchQuery: '',
    };
  },
  computed: {
    filteredUsers() {
      if (!this.searchQuery) {
        return this.users;
      }
      return Object.keys(this.users).filter(id =>
        this.users[id].toLowerCase().includes(this.searchQuery.toLowerCase())
      ).reduce((result, id) => {
        result[id] = this.users[id];
        return result;
      }, {});
    },
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
    async openUserDetails(userId) {
      try {
        this.selectedUser = await getUserById(userId);
        this.showUserDetailsDialog = true;
      } catch (error) {
        console.error('Error fetching user details:', error);
      }
    },
    closeUserDetailsDialog() {
      this.showUserDetailsDialog = false;
    },
    async removeUser(userId) {
      try {
        await deleteUserById(userId);
        this.fetchUsers();
        this.selectedUser = null;
        this.closeUserDetailsDialog();
      } catch (error) {
        console.error('Error deleting user:', error);
      }
    },
    getImageUrl(byteArray) {
      if (!byteArray) return '';
      const binaryString = window.atob(byteArray);
      const len = binaryString.length;
      const bytes = new Uint8Array(len);
      for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i);
      }
      const blob = new Blob([bytes], { type: 'image/jpeg' });
      return URL.createObjectURL(blob);
    },
    openEditDialog() {
      this.showEditDialog = true;
    },
    closeEditDialog() {
      this.showEditDialog = false;
    },
    async handleUpdate() {
      this.closeEditDialog();
      this.fetchUsers(); // Fetch updated users list
      if (this.selectedUser) {
        this.selectedUser = await getUserById(this.selectedUser.id); // Refresh selected user details
      }
    }
  },
};
</script>

<style>
.v-container {
  max-width: 600px;
  margin: 0 auto;
}
.scrollable-list {
  max-height: 400px;
  overflow-y: auto;
  border: 1px solid #ccc; 
}
img {
  max-width: 100%;
  height: auto;
}
</style>

