<template>
  <v-app>
    <v-main>
      <v-container>
        <ProfileForm @user-updated="handleUserUpdated" />
        <UserList ref="userList" />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import ProfileForm from './components/ProfileForm.vue';
import UserList from './components/UserList.vue';

export default {
  name: 'App',
  components: {
    ProfileForm,
    UserList,
  },
  methods: {
    handleUserUpdated() {
      this.$refs.userList.fetchUsers(); // Call fetchUsers method in UserList component
    }
  }
};
</script>
