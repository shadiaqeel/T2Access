// Core imports
import Vue from 'vue';
import router from './router/index';


Vue.config.performance = true


import App from './App.vue';

//Vue.config.devtools = true;

import store from './store/store';
//import { FontAwesomeIcon } from './icons';

// 3th imports
//import { sync } from 'vuex-router-sync';
import axios from 'axios';
import ElementUI from 'element-ui';
import locale from 'element-ui/lib/locale/lang/en';
//import locale from 'element-ui/lib/locale/lang/ar';

// CSS imports
import './styles/main.sass';

// Configurations
Vue.prototype.$http = axios;
Vue.use(ElementUI, { locale });
//Vue.component('icon', FontAwesomeIcon);
//sync(store, router);




new Vue({
    el: '#app',
    store,
    router,
    ...App
});