// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import store from "./vuex/store";
import axios from 'axios';
import VueAxios from 'vue-axios';

Vue.prototype.$axios = axios;
Vue.prototype.BASE_URL = 'https://aiape.snowphoenix.design';
Vue.use(ElementUI);
Vue.prototype.$store = store;

new Vue({
    el: '#app',
    render: h => h(App),
    router
})


