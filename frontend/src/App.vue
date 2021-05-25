<template>
    <el-container>
        <div class="background">
            <img :src="imgSrc" width="100%" height="100%" alt=""/>
        </div>
        <SideBar/>
        <transition
            name="zoom"
            enter-active-class="zoomInLeft"
            leave-active-class="zoomOutLeft">
            <router-view/>
        </transition>
    </el-container>
</template>

<script>
import Chat from './pages/Chat/Chat.vue'
import SideBar from "./components/SideBar/SideBar";
import Login from "./components/Login/Login";
import QuestionList from "./pages/QuestionList/QuestionList";

export default {
    data() {
        return {
            imgSrc: require('./assets/background1.jpg'),
            system: {
                win: false,
                mac: false,
                xll: false,
            }
        }
    },
    components: {
        SideBar,
        Chat,
        Login,
        QuestionList
    },
    created() {
        var p = navigator.platform;
        this.system.win = p.indexOf("Win") == 0;
        this.system.mac = p.indexOf("Mac") == 0;
        this.system.xll = p.indexOf("Xll") == 0;
        if (this.system.win || this.system.mac || this.system.xll) {
            alert("电脑端登录");
        } else {
            alert("移动端登录");
        }

        //在页面加载时读取localStorage里的状态信息
        if (sessionStorage.getItem("store")) {
            this.$store.replaceState(Object.assign({}, this.$store.state, JSON.parse(sessionStorage.getItem("store"))))
        }

        //在页面刷新时将vuex里的信息保存到localStorage里
        window.addEventListener("beforeunload", () => {
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

.el-main, .el-aside, .el-header {
    display: flex;
    padding: 0;
}

.el-container {
    align-items: center;
}

::-webkit-scrollbar {
    width: 0;
}

::-webkit-scrollbar-thumb:hover {
    background-color: #eaecf1;
    border-radius: 3px;
}

.chat {
    flex: 1 1 500px auto;
}

</style>
