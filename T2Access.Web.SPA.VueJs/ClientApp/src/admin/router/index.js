import Vue from 'vue';
import VueRouter from 'vue-router';
import routes from './routes';

// import i18n from 'admin/plugins/i18n';
// import axios from 'axios';
// import store from 'admin/store/store';

Vue.use(VueRouter);

import { getLocaleRegex, loadLanguageAsync } from 'admin/plugins/i18n';

const router = new VueRouter({
  mode: 'history',
  routes: [
    {
      path: `/:locale${getLocaleRegex()}/`,
      component: {
        render(c) {
          return c('router-view');
        }
      },
      beforeEnter(to, from, next) {
        loadLanguageAsync(to.params.locale).then(() => next());
      },
      props: route => ({ locale: route.params.locale || 'en' }),

      children: routes
    }
  ]
});

export default router;
