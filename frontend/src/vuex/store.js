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
        tagList: {},
        lastTokenTime: new Date(),
        logs: [{id: 2, content: '你好，我是AIApe!请先登录！'}]
    },
    mutations: {
        setUsername(state, value) {
            state.username = value;
        },
        setQuestionID(state, id) {
            state.questionID = id;
        },
        refreshToken(state, payload) {
            state.token = payload.token;
            state.lastTokenTime = payload.time;
            state.timeout = payload.timeout;
        },
        setAuth(state, auth) {
            state.auth = auth;
        },
        setTagList(state, tagList) {
            state.tagList = tagList;
        },
        addAImessage(state, message) {
            state.logs.push({
                id: 2,
                content: message
            })
        },
        addUserMessage(state, message) {
            state.logs.push({
                id: 1,
                content: message
            })
        }
    }
})
