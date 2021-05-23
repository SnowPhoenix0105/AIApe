<template>
    <el-container>
        <div class="background">
            <img :src="imgSrc" width="100%" height="100%" alt=""/>
        </div>
        <SideBar/>
        <Chat/>
        <transition name="el-fade-in">
            <Login v-show="$store.state.show.login"></Login>
        </transition>
        <transition name="el-fade-in">
            <QuestionList v-show="$store.state.show.questionList"></QuestionList>
        </transition>
    </el-container>
</template>

<script>
import Chat from './components/Chat/Chat.vue'
import SideBar from "./components/SideBar/SideBar";
import Login from "./components/Login/Login";
import QuestionList from "./components/QuestionList/QuestionList";

export default {
    data () {
        return {
            imgSrc: require('./assets/background1.jpg')
        }
    },
    components: {
        SideBar,
        Chat,
        Login,
        QuestionList
    },
    created() {
        //在页面加载时读取localStorage里的状态信息
        if (sessionStorage.getItem("store") ) {
            this.$store.replaceState(Object.assign({}, this.$store.state, JSON.parse(sessionStorage.getItem("store"))))
        }

        //在页面刷新时将vuex里的信息保存到localStorage里
        window.addEventListener("beforeunload",()=>{
            sessionStorage.setItem("store", JSON.stringify(this.$store.state))
        })

        this.$store.state.lastTokenTime = new Date();
    }
}
</script>

<style>
body {
    margin: 0;
    overflow: hidden;
}

.background {
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    z-index: -1;
    position: absolute;
    user-select: none;
}

.el-main, .el-aside, .el-header{
    display: flex;
    padding: 0;
}

.el-container {
    align-items: center;
}

::-webkit-scrollbar {
    width: 5px;
}

::-webkit-scrollbar-thumb:hover {
    background-color: #eaecf1;
    border-radius: 3px;
}

.chat {
    flex: 1 1 500px auto;
}

.shell {
    position: absolute;
    border: 1px solid lightgrey;
    left: 35px;
    top: 20vh;
    width: 30vw;
    height: 50vh;
    background-color: white;
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
    border-radius: 5px;
}

</style>
