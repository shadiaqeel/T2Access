import Vue from 'vue';
import Vuex from 'vuex';

// import logger from '../utils/helpers/vuex-logger';
import user from './modules/user';
import gate from './modules/gate';

Vue.use(Vuex);

//const debugMode = process.env.NODE_ENV !== 'production';

export default new Vuex.Store({
    modules: {
        user,
        gate
    },
   // strict: debugMode,
   // plugins: debugMode ? [logger()] : []
});