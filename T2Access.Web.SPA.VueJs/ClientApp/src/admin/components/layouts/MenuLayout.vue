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
          <router-link :to="{ name: 'home', params: { locale: locale.code } }" class="logo">
            <b>
              <!-- prettier-ignore -->
              t
              <span id="t">t</span>
              <span>Ac</span>cess
            </b>
          </router-link>
        </div>

        <div class="top-menu">
          <ul class="nav pull-right top-menu">
            <li style=" margin: 0px 20px;">
              <el-dropdown trigger="click">
                <span class="el-dropdown-link">
                  {{ locale.display }}
                  <i class="el-icon-arrow-down el-icon--right"></i>
                </span>
                <!-- <el-button size="mini" type="info">
                  lang
                  <i class="el-icon-arrow-down el-icon--right"></i>
                </el-button>-->
                <el-dropdown-menu slot="dropdown">
                  <el-dropdown-item v-for="(locale, index) in locales" :key="index">
                    <!-- <router-link
                      :to="{ name: $route.name , params:{locale: locale.code } }"
                    >{{locale.name}}</router-link>-->
                    <a :href="getLink(locale.code)">{{ locale.name }}</a>
                  </el-dropdown-item>
                </el-dropdown-menu>
              </el-dropdown>
            </li>
            <li>
              <a class="logout fa fa-sign-out btn" :href="`/${locale.code}/account/logout`">
                {{
                $t('nav.logout')
                }}
              </a>
            </li>
          </ul>
        </div>
      </nav>
      <!--header end-->
      <!--MAIN SIDEBAR MENU -->
      <!--sidebar start-->
      <aside>
        <div id="menu" :class="[{ active:  isActive }, 'nav-collapse']">
          <el-menu
            :router="true"
            :default-active="currentPage"
            class="el-menu-vertical-demo"
            @open="handleOpen"
            @close="handleClose"
            :collapse="!isActive "
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

            <el-menu-item index="home" :route="{ name: 'home', params: { locale: locale.code } }">
              <i class="el-icon-s-home"></i>
              <span slot="title">{{ $t('nav.dashboard') }}</span>
            </el-menu-item>
            <el-submenu index="account">
              <template slot="title">
                <i class="el-icon-monitor"></i>
                <span slot="title">{{ $t('nav.acountsManagment') }}</span>
              </template>
              <el-menu-item-group>
                <el-menu-item
                  index="user"
                  :route="{ name: 'user', params: { locale: locale.code } }"
                >{{ $t('nav.users') }}</el-menu-item>
                <el-menu-item
                  index="gate"
                  :route="{ name: 'gate', params: { locale: locale.code } }"
                >{{ $t('nav.gates') }}</el-menu-item>
              </el-menu-item-group>
            </el-submenu>
          </el-menu>
        </div>
      </aside>
    </section>
    <section id="main-content" :class="{ active:  isActive }">
      <section class="wrapper">
        <slot />
      </section>
      <!--main content end-->
      <!--footer start-->
      <footer class="site-footer navbar-fixed-bottom footer">
        <div class="text-center">
          <p>
            &copy; Copyrights
            <strong>T2 - business research and development</strong> All Rights
            Reserved
          </p>
        </div>
      </footer>
    </section>
  </div>
</template>




// ****************************************************************************************************************
// *************************************************** { script } *************************************************
// ****************************************************************************************************************


<script>
import { SUPPORTED_LOCALES } from "admin/constants/locales";

export default {
  name: "MenuLayout",
  data() {
    return {
      isActive: true,
      path: "/",
      locales: SUPPORTED_LOCALES
    };
  },
  computed: {
    currentPage() {
      return this.$route.name;
    },
    locale() {
      return this.$store.getters.locale;
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
    },
    getLink(code) {
      if (this.locale.code != code)
        return this.$router.resolve({
          name: this.$route.name,
          params: { locale: code }
        }).href;
      else "";
    }
  }
};
</script>

// ****************************************************************************************************************
// *************************************************** { style } **************************************************
// ****************************************************************************************************************
<style lang="scss">
$main-color: #4ecdc4;

.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 205px;
  height: 100%;
  min-height: 400px;
  margin-top: 60px;
  position: fixed;
  text-align: justify;
}

html[dir="ltr"] {
  @import "../../styles/en/style";

  #main-content {
    margin-left: 63px;
    transition: all 0.3s ease-in-out;

    height: 100%;
    left: 0;
    right: 0;
    position: absolute;
    min-height: 100%;
    &.active {
      margin-left: 204.2px;
      transition: all 0.3s ease-in-out;
    }
    .pull-away {
      float: right;
    }
  }
  #menu {
    .el-menu {
      border-right: none;
    }
    .el-menu--collapse {
      width: 65px;
    }

    i.el-submenu__icon-arrow.el-icon-arrow-down {
      right: 6px;
    }
  }
}

html[dir="rtl"] {
  @import "../../styles/ar/style";

  #main-content {
    margin-right: 63px;
    transition: all 0.3s ease-in-out;
    height: 100%;
    left: 0;
    right: 0;
    position: absolute;
    min-height: 100%;
    &.active {
      margin-right: 204.2px;
      transition: all 0.3s ease-in-out;
    }
    .content-panel {
      text-align: justify;
    }
    .el-button + .el-button {
      margin-right: 10px;
    }
    .pull-away {
      float: left;
    }
  }

  #menu {
    .el-menu {
      border-right: none;
    }
    .el-menu--collapse {
      width: 65px;
    }
    i.el-submenu__icon-arrow.el-icon-arrow-down {
      right: auto;
      left: 20px;
    }
  }
}

//  #main-content {
//   margin-left: 63px;
//   transition: all 0.3s ease-in-out;
// }

// #main-content.active {
//   margin-left: 204.2px;
//   transition: all 0.3s ease-in-out;
// }
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
