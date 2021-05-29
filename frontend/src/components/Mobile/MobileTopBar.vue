<template>
    <el-header height="20%">
        <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                   size="large" @click.native="login"></el-avatar>
        <i class="el-icon-chat-dot-round" :class="(this.index === 0)? 'selected' : 'unselected'" @click="showChat"></i>
        <i class="el-icon-edit" :class="(this.index === 1)? 'selected' : 'unselected'" @click="showRaiseQuestion"></i>
        <i class="el-icon-notebook-2" :class="(this.index === 2)? 'selected' : 'unselected'"
           @click="showQuestionList"></i>
        <i class="el-icon-paperclip" :class="(this.index === 3)? 'selected' : 'unselected'"
           @click="showQuestionDetail"></i>
    </el-header>
</template>

<script>
export default {
    name: "MobileBottomBar",
    data() {
        return {
            // index: 0,
        }
    },
    methods: {
        showChat() {
            // alert('herre');
            this.$store.state.mobileStatus = 'chat';
        },
        showRaiseQuestion() {
            this.$store.state.mobileStatus = 'raiseQuestion';
        },
        showQuestionList() {
            this.$store.state.mobileStatus = 'questionList';
        }
    }
}
</script>

<style scoped>

.el-header {
    background: rgba(0, 0, 0, 0.4);
    width: 100%;
    flex-direction: row;
    align-items: center;
}

.el-avatar {
    margin-top: 2%;
    margin-bottom: 1%;
    cursor: pointer;
}

i {
    margin-left: 10%;
    margin-right: 0;
    font-size: 150%;
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
