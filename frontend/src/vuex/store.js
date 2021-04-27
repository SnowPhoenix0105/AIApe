import Vue from 'vue'
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        usernameFresh: false,
        username: '',
    },
    mutations: {
        setUsername(state, value) {
            state.username = value;
        }
    }
})
