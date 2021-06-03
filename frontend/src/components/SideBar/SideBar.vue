<template>
    <el-aside width="5vw">
        <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                   size="large" @click.native="login"></el-avatar>
        <i class="el-icon-chat-dot-round" :class="(index === 0)? 'selected' : 'unselected'" @click="changePage(0);"></i>
        <i class="el-icon-edit" :class="(index === 1)? 'selected' : 'unselected'" @click="changePage(1);"></i>
        <i class="el-icon-notebook-2" :class="(index === 2)? 'selected' : 'unselected'" @click="changePage(2);"></i>
        <i class="el-icon-paperclip" :class="(index === 3)? 'selected' : 'unselected'" @click="changePage(3);" v-show="questionDetailExist"></i>
        <i class="el-icon-search" :class="(index === 4)? 'selected' : 'unselected'" @click="changePage(4);" v-show="searchResultExist"></i>
        <i class="el-icon-cpu" :class="(index === 5)? 'selected' : 'unselected'" @click="changePage(5);"></i>
    </el-aside>
</template>

<script>
export default {
    data() {
        return {
            avatarSrc: require('../../assets/bot.jpg'),
        }
    },
    computed: {
        index() {
            return this.$store.state.routerIndex;
        },
        questionDetailExist() {
            return this.$store.state.questionID > 0;
        },
        searchResultExist() {
            return this.$store.state.searchResult.length > 0;
        }
    },
    methods: {
        login() {
            if (this.$store.state.username === '') {
                this.$store.state.show.login = true;
            }
            else {
                this.$changePage(6);
            }
        },
        changePage(index) {
            this.$changePage(index);
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
