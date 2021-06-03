<template>
    <el-container>
        <el-header height=5vh>
            AIApe
        </el-header>
        <el-header height=5vh class="back">
                <el-link v-on:click="goBack">返回</el-link>
        </el-header>
        <h1>问题详情页面{{ this.$store.state.questionID }}</h1>
    </el-container>
</template>

<script>
export default {
    name: "MobileQuestionDetail",
    methods: {
        goBack() {
            this.$store.state.mobileStatus = 'questionList';
        }
    }
}
</script>

<style scoped>

.el-header {
    border-bottom: 1px solid #eaecf1;
    width: 100vw;
    align-items: center;
    padding: 5%;
    /*height: 5vh;*/
}

.back {
    border-bottom: 0;
    width: 100vw;
    align-items: center;
    padding: 5%;
}

</style>
