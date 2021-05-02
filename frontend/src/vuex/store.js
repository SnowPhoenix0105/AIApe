import Vue from 'vue'
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        questionID: 0,
        username: '',
        token: '',
        auth: 0,
        timeout: 0,
        tagList: {}
    },
    mutations: {
        setUsername(state, value) {
            state.username = value;
        },
        setQuestionID(state, id) {
            state.questionID = id;
        },
        setToken(state, token) {
            state.token = token;
        },
        setAuth(state, auth) {
            state.auth = auth;
        },
        setTimeout(state, timeout) {
            state.timeout = timeout;
        },
        setTagList(state, tagList) {
            state.tagList = tagList;
        }
    }
})
