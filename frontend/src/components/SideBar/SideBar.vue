<template>
    <el-aside width="5vw">
        <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                   size="large" @click.native="login"></el-avatar>
        <i class="el-icon-chat-dot-round" :class="(this.index === 0)? 'selected' : 'unselected'" @click="showChat"></i>
        <i class="el-icon-edit" :class="(this.index === 1)? 'selected' : 'unselected'" @click="showRaiseQuestion"></i>
        <i class="el-icon-notebook-2" :class="(this.index === 2)? 'selected' : 'unselected'" @click="showQuestionList"></i>
        <i class="el-icon-paperclip" :class="(this.index === 3)? 'selected' : 'unselected'" @click="showQuestionDetail"></i>
    </el-aside>
</template>

<script>
export default {
    data() {
        return {
            avatarSrc: require('../../assets/bot.jpg'),
            index: 0
        }
    },
    methods: {
        login() {
            this.$store.state.show.login = true;
        },
        showChat() {
            this.$router.replace('/chat');
            this.index = 0;
        },
        showRaiseQuestion() {
            this.$router.replace('/raiseQuestion');
            this.index = 1;
        },
        showQuestionList() {
            this.$router.replace('/questionList');
            this.index = 2;
        },
        showQuestionDetail() {
            this.$router.replace('/questionDetail');
            this.index = 3;
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
    font-size: 30px;
    cursor: pointer;
}

i:hover {
    color: lightgrey;
}

.selected {
    color: lightgrey;
}

.unselected {
    color: black;
}
</style>
