import Vue from 'vue';
import VueI18n from 'vue-i18n';
import axios from 'axios';
import store from 'admin/store/store';

Vue.use(VueI18n);

import { SUPPORTED_LOCALES } from 'admin/constants/locales';

const loadedLanguages = ['en']; // our default language that is preloaded

// Returns locale configuration
function getLocale(locale = 'en') {
  return SUPPORTED_LOCALES.find(loc => loc.code === locale);
}

function loadLocaleMessages() {
  const locales = require.context(
    '../locales',
    true,
    /[A-Za-z0-9-_,\s]+\.json$/i
  );
  const messages = {};
  locales.keys().forEach(key => {
    const matched = key.match(/([A-Za-z0-9-_]+)\./i);
    if (matched && matched.length > 1) {
      const locale = matched[1];
      messages[locale] = locales(key);
    }
  });
  return messages;
}

export const i18n = new VueI18n({
  silentTranslationWarn: true,
  locale: process.env.VUE_APP_I18N_LOCALE || 'en',
  fallbackLocale: process.env.VUE_APP_I18N_FALLBACK_LOCALE || 'en',
  messages: loadLocaleMessages()
});

function setI18nLanguage(locale) {
  i18n.locale = locale.code;
  console.log(locale);
  store.commit('setLocale', locale);
  axios.defaults.headers.common['Accept-Language'] = locale.code;
  axios.defaults.baseURL = `/${locale.code}/`;
  document.querySelector('html').setAttribute('lang', locale.code);
  document.querySelector('html').setAttribute('dir', locale.dir);
  return locale;
}

// Creates regex (en|ar)
export function getLocaleRegex() {
  let reg = '';
  SUPPORTED_LOCALES.forEach((locale, index) => {
    reg = `${reg}${locale.code}${
      index !== SUPPORTED_LOCALES.length - 1 ? '|' : ''
    }`;
  });
  return `(${reg})`;
}

export function loadLanguageAsync(lang) {
  const locale = getLocale(lang);

  //// If the same language
  // if (i18n.locale === lang) {
  //     return Promise.resolve(setI18nLanguage(locale))
  // }

  //// If the language was already loaded
  // if (loadedLanguages.includes(lang)) {
  //     return Promise.resolve(setI18nLanguage(locale))
  // }

  return axios.get(locale.translations).then(res => {
    i18n.setLocaleMessage(locale.code, res.data || {});
    return setI18nLanguage(locale);
  });

  //// If the language hasn't been loaded yet
  // return import ( /* webpackChunkName: "lang-[request]" */ `@/i18n/messages/${lang}.js`).then(
  //     messages => {
  //         i18n.setLocaleMessage(lang, messages.default)
  //         loadedLanguages.push(lang)
  //         return setI18nLanguage(lang)
  //     }
  // )
}
