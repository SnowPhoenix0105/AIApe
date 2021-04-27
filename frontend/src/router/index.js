import Vue from 'vue'
import Router from 'vue-router'
import Register from '../pages/Register/Register.vue'
import Login from '../pages/Login/Login.vue'
import QuestionList from "../pages/QuestionList/QuestionList";

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
      path: '/questionList',
      component: QuestionList
    },
    {
      path: '/',
      redirect: 'login'
    }
  ]
})
