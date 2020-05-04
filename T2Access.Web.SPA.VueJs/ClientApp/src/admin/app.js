//? Core imports
import Vue from 'vue';
import Vuex from 'vuex';
import router from './router/index';
import { i18n } from './plugins/i18n';

Vue.config.productionTip = false;
Vue.config.performance = true;
//Vue.config.devtools = true;


import App from './App.vue';
import store from './store/store';
//import { FontAwesomeIcon } from './icons';

//? 3th imports
import axios from 'axios';
import ElementUI from 'element-ui';
import locale from 'element-ui/lib/locale/lang/en';
import Notifications from 'vue-notification';
import velocity from 'velocity-animate';
// import { sync } from 'vuex-router-sync';
//import { Loading } from 'element-ui';
//import locale from 'element-ui/lib/locale/lang/ar';


//? CSS imports
import "normalize.css";
import './styles/main.sass';




//? Configurations
Vue.prototype.$http = axios;
// Vue.prototype.$notification = Notification;
// Vue.prototype.$loading = Loading.service({ fullscreen: true });
// Vue.use(ElementUI, { locale });
Vue.use(ElementUI, {
    size: 'medium', // set element-ui default size
    i18n: (key, value) => i18n.t(key, value)
});
Vue.use(Vuex);
Vue.use(Notifications, { velocity });

//Vue.component('icon', FontAwesomeIcon);
// sync(store, router);

new Vue({
    el: '#app',
    store,
    router,
    i18n,
    ...App
});