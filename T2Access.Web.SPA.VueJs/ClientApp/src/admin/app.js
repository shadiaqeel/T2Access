// Core imports
import Vue from 'vue';
import Vuex from 'vuex'
import router from './router/index';


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

import locale from 'element-ui/lib/locale/lang/en';
//import locale from 'element-ui/lib/locale/lang/ar';

// CSS imports
import './styles/main.sass';


Vue.use(ElementUI, { locale });
Vue.use(Vuex);

// Configurations
Vue.prototype.$http = axios;
// Vue.prototype.$loading = Loading.service({ fullscreen: true });
// Vue.prototype.$myStore = store;


//Vue.component('icon', FontAwesomeIcon);
// sync(store, router);




new Vue({
    el: '#app',
    store,
    router,
    ...App
});