import Vue from 'vue';
import VueRouter from 'vue-router';
import routes from './routes';

import i18n from 'admin/plugins/i18n';
import axios from 'axios';
import store from 'admin/store/store';




Vue.use(VueRouter);


import { SUPPORTED_LOCALES } from 'admin/constants/locales'
// Creates regex (en|ar)
function getLocaleRegex() {
    let reg = ''
    SUPPORTED_LOCALES.forEach((locale, index) => {
        reg = `${reg}${locale.code}${index !== SUPPORTED_LOCALES.length - 1 ? '|' : ''}`
    })
    return `(${reg})`
}

// Returns locale configuration
function getLocale(locale = 'en') {
    return SUPPORTED_LOCALES.find(loc => loc.code === locale);
}


const router = new VueRouter({
    mode: 'history',
    routes: [{
        path: `/:locale${getLocaleRegex()}?/`,
        component: {
            render(c) { return c('router-view') }
        },
        beforeEnter(to, from, next) {
            const locale = getLocale(to.params.locale);
            store.commit('setLocale', locale);
            console.log(locale);

            axios.get(locale.translations).then(res => {
                i18n.setLocaleMessage(locale.code, res.data || {});
            }).catch(() => {
                // TODO handle error
                console.log(e);


            }).finally(() => {
                i18n.locale = locale.code;
                next();
            });
        },

        children: routes
    }]
});

export default router;