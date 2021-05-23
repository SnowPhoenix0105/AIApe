<template>
    <el-aside width="5vw">
        <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                   size="large" @click.native="login"></el-avatar>
        <i class="el-icon-notebook-2" @click="showQuestionList"></i>
    </el-aside>
</template>

<script>
export default {
    data() {
        return {
            avatarSrc: require('../../assets/bot.jpg')
        }
    },
    methods: {
        login() {
            this.$store.state.show.login = true;
        },
        showQuestionList() {
            this.$store.state.show.questionList = !this.$store.state.show.questionList;
        }
    }
}
</script>

<style scoped>

.el-aside {
    background: rgba(0, 0, 0, 0.4);
    height: 100vh;
    flex-direction: column;
    align-items: center;
}

.el-avatar {
    margin-top: 20px;
    cursor: pointer;
}

i {
    margin-top: 40px;
    color: black;
    font-size: 40px;
    cursor: pointer;
}

i:hover {
    color: lightgrey;
}
</style>
