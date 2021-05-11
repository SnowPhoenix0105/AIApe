<template>
    <div>
        <div class="left">
            <Chat/>
        </div>
        <div class="right">
            <router-view/>
        </div>
    </div>
</template>

<script>
import Chat from './components/Chat/Chat.vue'

export default {
    components: {
        Chat
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
/*#app {*/
/*  font-family: 'Avenir', Helvetica, Arial, sans-serif;*/
/*  -webkit-font-smoothing: antialiased;*/
/*  -moz-osx-font-smoothing: grayscale;*/
/*  text-align: center;*/
/*  color: #2c3e50;*/
/*  margin-top: 60px;*/
/*}*/

html, body, #app, .left, .right {
    height: 100%;
    width: 100%;
    padding: 0;
    margin: 0;
}

.left {
    position: absolute;
    left: 0;
    top: 0;
    width: 40%;
    border-right: 1px solid lightgrey;
}

.right {
    position: absolute;
    left: 40%;
    top: 0;
    width: 60%;
}

::-webkit-scrollbar {
    width: 5px;

}

::-webkit-scrollbar-thumb:hover {
    background-color: #eaecf1;
    border-radius: 3px;
}
</style>
