import Vue from 'vue'
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        questionID: 0,
        username: '',
    },
    mutations: {
        setUsername(state, value) {
            state.username = value;
        },
        setQuestionID(state, id) {
            state.questionID = id;
        }
    }
})
