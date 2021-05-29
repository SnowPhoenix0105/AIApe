import Vue from 'vue';
import Router from 'vue-router';
import Register from '../pages/Register/Register.vue';
import Login from '../components/Login/Login.vue';
import QuestionList from "../pages/QuestionList/QuestionList";
import QuestionDetail from "../pages/QuestionDetail/QuestionDetail";
import PersonalCenter from "../pages/PersonalCenter/PersonalCenter";
import Administration from "../pages/Administration/Administration";
import Chat from "../pages/Chat/Chat";
import RaiseQuestion from "../pages/RaiseQuestion/RaiseQuestion";
import SearchResult from "../pages/SearchResult/SearchResult";

Vue.use(Router)

export default new Router({
    scrollBehavior (to, from) {
        if (from.name === 'questionList') {
            from.meta.savedPosition = document.getElementById('scroll-body').scrollTop;
        }
        if (to.name === 'questionList' && to.meta.savedPosition) {
            document.getElementById('scroll-body').scrollTop = to.meta.savedPosition;
        }
    },
    routes: [
        {
            path: '/',
            redirect: 'chat',
        },
        {
            path: '/questionList',
            component: QuestionList,
            meta: {keepAlive: true},
            name: 'questionList'
        },
        {
            path: '/questionDetail',
            component: QuestionDetail,
            meta: {keepAlive: false},
            name: 'questionDetail'
        },
        {
            path: '/raiseQuestion',
            component: RaiseQuestion,
            meta: {keepAlive: true},
            name: 'raiseQuestion'
        },
        {
            path: '/personalCenter',
            component: PersonalCenter
        },
        {
            path: '/administration',
            component: Administration
        },
        {
            path: '/chat',
            component: Chat,
            meta: {keepAlive: true},
            name: 'chat'
        },
        {
            path: '/searchResult',
            component: SearchResult,
            name: 'searchResult'
        }
    ]
})
