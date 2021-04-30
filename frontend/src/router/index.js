import Vue from 'vue'
import Router from 'vue-router'
import Register from '../pages/Register/Register.vue'
import Login from '../pages/Login/Login.vue'
import QuestionList from "../pages/QuestionList/QuestionList";
import QuestionDetail from "../pages/QuestionDetail/QuestionDetail";
import PersonalCenter from "../pages/PersonalCenter/PersonalCenter";

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
            component: QuestionList
        },
        {
            path: '/questionDetail',
            component: QuestionDetail
        },
        {
            path: '/personalCenter',
            component: PersonalCenter
        }
    ]
})
