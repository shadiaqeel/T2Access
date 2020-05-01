//"use strict";

//const fs = require('fs')
const path = require("path");






//// We search for app.js or app.ts files inside ClientApp/src/{miniSpaName} folder and make those as entries. Convention over configuration
//var appEntryFiles = {}
//const applicationBasePath = "./src/";

//fs.readdirSync(applicationBasePath).forEach(function (name) {

//    //let spaEntryPoint = applicationBasePath + name + '/app.ts'

//    //if (fs.existsSync(spaEntryPoint)) {
//    //  appEntryFiles[name] = spaEntryPoint
//    //}

//    let spaEntryPoint = applicationBasePath + name + '/app.js'
//    if (fs.existsSync(spaEntryPoint)) {
//        appEntryFiles[name] = spaEntryPoint
//    }

//})

//// Add main site.scss file with Bulma(or any other source by choice)
//appEntryFiles["vendor"] = [
//    path.resolve(__dirname, applicationBasePath + 'common/design/site.scss'),
//]








module.exports = {


    // where to output built files
    outputDir: 'dist',

    // where to put static assets (js/css/img/font/...)
    assetsDir: 'assets',

    filenameHashing: false,

    // whether to use eslint-loader for lint on save.
    // valid values: true | false | 'error'
    // when set to 'error', lint errors will cause compilation to fail.
    lintOnSave: true,

    // use the full build with in-browser compiler?
    // https://vuejs.org/v2/guide/installation.html#Runtime-Compiler-vs-Runtime-only
    runtimeCompiler: false,

    // babel-loader skips `node_modules` deps by default.
    // explicitly transpile a dependency with this option.
    transpileDependencies: [ /* string or regex */ ],

    // generate sourceMap for production build?
    productionSourceMap: false,

    // tweak internal webpack configuration.
    // see https://github.com/vuejs/vue-cli/blob/dev/docs/webpack.md
    chainWebpack: config => {
        config.resolve.alias.set('admin', path.join(__dirname, './src/admin'));
    },
    configureWebpack: () => {},

    // CSS related options
    css: {
        // extract CSS in components into a single CSS file (only in production)
        // can also be an object of options to pass to extract-text-webpack-plugin
        extract: true,

        // Enable CSS modules for all css / pre-processor files.
        // This option does not affect *.vue files.
        // modules: false,

        // enable CSS source maps?
        sourceMap: false,

        // pass custom options to pre-processor loaders. e.g. to pass options to
        // sass-loader, use { sass: { ... } }
        loaderOptions: {}
    },

    // use thread-loader for babel & TS in production build
    // enabled by default if the machine has more than 1 cores
    parallel: require("os").cpus().length > 1,

    // options for the PWA plugin.
    // see https://github.com/vuejs/vue-cli/tree/dev/packages/%40vue/cli-plugin-pwa

    // configure webpack-dev-server behavior
    devServer: {
        port: "8888",
        https: false,
        host: "localhost",
        open: "true",
        proxy: null
    },

    // options for 3rd party plugins
    pluginOptions: {
      i18n: {
        locale: 'en',
        fallbackLocale: 'en',
        localeDir: 'locales',
        enableInSFC: false
      }
    },
    pages: {
        admin: {
            // entry for the page
            entry: "./src/admin/app.js",
            filename: "html/admin.html"

        },
        common: {
            entry: "./src/common/design/site.scss",
            filename: "html/common.html"


        }
    }

};
