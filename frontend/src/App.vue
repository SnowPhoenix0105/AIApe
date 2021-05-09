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
