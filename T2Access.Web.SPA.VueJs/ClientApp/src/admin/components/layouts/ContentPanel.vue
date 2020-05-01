<template >
  <component :is="layout">
    <div class="row mt">
      <div class="col-md-12">
        <div class="content-panel">
          <transition>
            <keep-alive>
              <router-view></router-view>
            </keep-alive>
          </transition>
        </div>
      </div>
    </div>
    <notifications position="bottom right" group="main" animation-type="velocity" />
  </component>
</template>

<script>
import MenuLayout from "./MenuLayout.vue";

export default {
  name: "ContentPanel",
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

<style lang="sass" scoped>
</style>
