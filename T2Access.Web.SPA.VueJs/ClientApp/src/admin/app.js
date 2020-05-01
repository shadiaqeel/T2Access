// Core imports
import Vue from 'vue'
import Vuex from 'vuex'
import router from './router/index'
import i18n from './plugins/i18n'

// import { Notification } from "admin/utils/helper/notification";
import Notifications from 'vue-notification'
import velocity from 'velocity-animate'




Vue.config.productionTip = false
Vue.config.performance = true



//Vue.config.devtools = true;

import App from './App.vue';
import store from './store/store';
//import { FontAwesomeIcon } from './icons';

// 3th imports
// import { sync } from 'vuex-router-sync';
import axios from 'axios';
import ElementUI from 'element-ui';
//import { Loading } from 'element-ui';

import localeEn from 'element-ui/lib/locale/lang/en';
import localeAr from 'element-ui/lib/locale/lang/ar';

// CSS imports
import './styles/main.sass';


Vue.use(ElementUI, {
    localeEn
});
Vue.use(Vuex);
Vue.use(Notifications, { velocity })


// Configurations
Vue.prototype.$http = axios;
// Vue.prototype.$notification = Notification;
// Vue.prototype.$loading = Loading.service({ fullscreen: true });
// Vue.prototype.$myStore = store;


//Vue.component('icon', FontAwesomeIcon);
// sync(store, router);




new Vue({
    el: '#app',
    store,
    router,
    i18n,
    ...App
});