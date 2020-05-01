import Vue from 'vue';
import Vuex from 'vuex';

// import logger from '../utils/helpers/vuex-logger';
import user from './modules/user';
import gate from './modules/gate';

Vue.use(Vuex);

//const debugMode = process.env.NODE_ENV !== 'production';



const state = {
    locale: {
        code: 'en',
        base: '',
        flag: 'us',
        name: 'English',
        translations: '/translations/en.json'
    }
}
const getters = {
    locale: state => state.locale,

}

const mutations = {
    setLocale(state, locale) {
        state.locale = locale;
    }
}

export default new Vuex.Store({
    state,
    getters,
    mutations,
    modules: {
        user,
        gate
    },
    // strict: debugMode,
    // plugins: debugMode ? [logger()] : []
});