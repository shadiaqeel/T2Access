<template>
  <component :is="layout">
    <router-view></router-view>
    <notifications position="bottom right" group="main" animation-type="velocity" />
  </component>
</template>

<script>
import MenuLayout from "admin/components/layouts/MenuLayout.vue";

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

<style lang="scss">
</style>
