<template>
  <v-app>
    <v-container>
      <ProfileForm />
      <UserList />
    </v-container>
  </v-app>
</template>

<script>
import ProfileForm from './components/ProfileForm.vue';
import UserList from './components/UserList.vue';

export default {
  name: 'App',
  components: {
    ProfileForm,
    UserList
  }
};
</script>

<style>
@import 'vuetify/dist/vuetify.min.css';

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
