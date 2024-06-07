<template>
  <div id="app">
      <ProfileForm />
  </div>
</template>

<script>
  /* eslint-disable vue/no-unused-components */
  import ProfileForm from './components/ProfileForm.vue';

  export default {
      name: 'App',
      components: {
          ProfileForm
      }
  };
</script>

<style>
  #app {
      font-family: Avenir, Helvetica, Arial, sans-serif;
      -webkit-font-smoothing: antialiased;
      -moz-osx-font-smoothing: grayscale;
      text-align: center;
      color: #2c3e50;
      margin-top: 60px;
  }
</style>
