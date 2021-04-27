import Vue from 'vue'
import Router from 'vue-router'
import Register from '../pages/Register/Register.vue'
import Login from '../pages/Login/Login.vue'
import Questions from "../pages/QuestionList/Questions";

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/register',
            component: Register
        },
        {
            path: '/login',
            component: Login
        },
        {
            path: '/',
            redirect: 'login'
        },
        {
            path: '/questionList',
            component: Questions
        }
    ]
})
