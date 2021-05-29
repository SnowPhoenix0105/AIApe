// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import store from "./vuex/store";
import axios from 'axios';
import 'github-markdown-css/github-markdown.css';
import 'vue2-animate/dist/vue2-animate.min.css';
import mavonEditor from 'mavon-editor';
import 'mavon-editor/dist/css/index.css';
import VueAxios from 'vue-axios';
import directive from "element-ui/packages/popover/src/directive";

Vue.prototype.$axios = axios;
// const BASE_URL = 'https://aiape.snowphoenix.design';
const BASE_URL = 'http://test.snowphoenix.design';
Vue.prototype.BASE_URL = BASE_URL;
Vue.use(ElementUI);
Vue.use(mavonEditor);
Vue.prototype.$store = store;

new Vue({
    el: '#app',
    render: h => h(App),
    router
})

axios.interceptors.response.use(response => {
    // 几种不需要刷新token的情况
    if (store.state.token === '' || response.data.message === 'token fresh' ||
        new Date().getTime() - store.state.lastTokenTime < store.state.timeout / 2) {
        return response;
    }

    let current = new Date();
    let existTime = (current.getTime() - store.state.lastTokenTime.getTime()) / 1000;

    if (existTime > store.state.timeout) {
        alert('登录超时!');
        router.replace('/login');
    }
    else {
        axios.post( BASE_URL + '/api/user/fresh', {
            token: store.state.token
        })
            .then(function (ret) {
                if (ret.data.state === 'success') {
                    store.commit('refreshToken', {
                        token: ret.data.token,
                        time: new Date(),
                        timeout: ret.data.timeout
                    })
                }
            })
            .catch(function (error) {
                console.log(error);
            })
        return response;
    }
})

