<template>
  <component :is="layout">
    <router-view></router-view>
  </component>
</template>

<script>
import MenuLayout from "./components/layouts/MenuLayout.vue";

export default {
  name: "App",
  data() {
    return {
      layout: "menu-layout"
    };
  },
  components: {
    MenuLayout
  }
};
</script>

<style lang="scss" >
</style>

