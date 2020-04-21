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
          ></div>
          <!--logo-->
          <a href="#" class="logo">
            <b>
              t
              <span id="t">t</span>
              <span>Ac</span>cess
            </b>
          </a>
        </div>

        <div class="top-menu">
          <ul class="nav pull-right top-menu">
            <li>
              <button class="logout fa fa-sign-out btn">logout</button>
            </li>
          </ul>
        </div>
      </nav>
      <!--header end-->
      <!--MAIN SIDEBAR MENU -->
      <!--sidebar start-->
      <aside>
        <div id="sidebar" class="nav-collapse">
          <!-- sidebar menu start-->
          <ul class="sidebar-menu" id="nav-accordion">
            <li class="mt">
              <router-link :to="{ name: 'home' }">
                <i class="fa fa-dashboard"></i>
                <span>Dashboard</span>
              </router-link>
            </li>
            <li class="sub-menu">
              <router-link :to="{ name: 'Account' }">
                <i class="fa fa-desktop"></i>
                <span>Accounts Managment</span>
              </router-link>

              <ul class="sub">
                <li class>
                  <a href="#">Users</a>
                </li>
                <li class>
                  <a href="#">Gates</a>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </aside>
    </section>
    <section id="main-content">
      <section class="wrapper"></section>



      <slot />



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
  mounted() {
    let externalScript = document.createElement("script");
    externalScript.setAttribute("src", "~/js/admin/en/scripts.js");
    document.head.appendChild(externalScript);
  }
};
</script>

<style lang="scss" >
@import "../../styles/style.css";
@import "../../styles/style-responsive.css";
</style>

