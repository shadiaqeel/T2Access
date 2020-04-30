<template>
  <div>
    <!--<link rel="icon" type="image/png" href="~/images/admin/favicon.PNG" />-->
    <!--<link href="~/lib/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />-->

    <section id="container">
      <!-- TOP BAR CONTENT & NOTIFICATIONS -->
      <!--header start-->
      <nav class="navbar header black-bg">
        <div class="sidebar-toggle-box">
          <div
            class="fa fa-bars tooltips"
            data-placement="right"
            data-original-title="Toggle Navigation"
            @click="isActive = !isActive"
          ></div>
          <!--logo-->
          <a href="#" class="logo">
            <b>
              t<span id="t">t</span><span>Ac</span>cess
            </b>
          </a>
        </div>

        <div class="top-menu">
          <ul class="nav pull-right top-menu">
            <li>
              <a class="logout fa fa-sign-out btn" href="/logout">logout</a>
            </li>
          </ul>
        </div>
      </nav>
      <!--header end-->
      <!--MAIN SIDEBAR MENU -->
      <!--sidebar start-->
      <aside>
        <div
          id="menu"
          :class="[{ active: !isCollapse || isActive },'nav-collapse']"
          @mouseover="isCollapse = false"
          @mouseleave="isCollapse = true"
        >
          <el-menu
            :router="true"
            :default-active="currentPage"
            class="el-menu-vertical-demo"
            @open="handleOpen"
            @close="handleClose"
            :collapse=" !isActive && isCollapse"
            collapse-transition
            background-color="#2f323a"
            text-color="#fff"
            active-text-color="#4ecdc4"
            style=" 
    position: fixed;
    height: 100%;
    margin-top: 60px;
  "
          >
            <br />

            <el-menu-item index="home" :route="{name:'home'}">
              <i class="el-icon-s-home"></i>
              <span slot="title">Dashboard</span>
            </el-menu-item>
            <el-submenu index="account">
              <template slot="title">
                <i class="el-icon-monitor"></i>
                <span slot="title">Accounts Managment</span>
              </template>
              <el-menu-item-group>
                <el-menu-item index="user" :route="{name:'user'}">Users</el-menu-item>
                <el-menu-item index="gate" :route="{name:'gate'}">Gates</el-menu-item>
              </el-menu-item-group>
            </el-submenu>
          </el-menu>
        </div>
      </aside>
    </section>
    <section id="main-content" :class="{ active: !isCollapse || isActive }">
      <section class="wrapper">
        <slot />
      </section>
      <!--main content end-->
      <!--footer start-->
      <footer class="site-footer navbar-fixed-bottom footer">
        <div class="text-center">
          <p>
            &copy; Copyrights
            <strong>T2 - business research and development</strong> All Rights Reserved
          </p>
        </div>
      </footer>
    </section>
  </div>
</template>





<script>
export default {
  name: "MenuLayout",
  data() {
    return {
      isCollapse: false,
      isActive: true
    };
  },
  computed: {
    currentPage() {
      return this.$route.name;
    }
  },
  mounted() {
    // let externalScript = document.createElement("script");
    // externalScript.setAttribute("src", "/js/admin/en/scripts.js");
    // document.body.appendChild(externalScript);
  },
  methods: {
    handleOpen(key, keyPath) {
      console.log(key, keyPath);
    },
    handleClose(key, keyPath) {
      console.log(key, keyPath);
    },
    getRoute(_name) {
      return this.$router.resolve({ name: _name });
    },
    toggle: function() {
      this.isActive = !this.isActive;
    }
  }
};
</script>

<style lang="scss" >
@import "../../styles/style.css";
@import "../../styles/style-responsive.css";

.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 205px;
  height: 100%;
  min-height: 400px;
  margin-top: 60px;
  position: fixed;
}

#main-content {
  margin-left: 63px;
  transition: all 0.3s ease-in-out;
}

#main-content.active {
  margin-left: 204.2px;
  transition: all 0.3s ease-in-out;
}
// $screen-md-min
@media only screen and (max-width: 768px) {
  #menu .el-menu {
    display: none;
  }

  #menu.active {
    margin: 70px 0;
    visibility: visible;
    opacity: 0.98;
    transition: all 0.5s ease-out;

    .el-menu-vertical .el-menu-item {
      text-align: center;
      float: none;
      display: block;
      height: 100%;
      width: 100%;
      border-top: 1px solid #eaeaeb;
      font-size: 18px;
    }
  }
}
</style>

