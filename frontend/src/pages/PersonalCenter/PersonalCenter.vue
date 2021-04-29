<template>
    <el-container style="height: 100%">
        <el-page-header @back="goBack" content="个人中心">
        </el-page-header>
        <el-header style="height: auto">
            <h1 align="center">基本信息</h1>
            <div class="header">
                <p>我的昵称：{{ nickName }}</p>
                <p>我的邮箱：{{ email }}</p>
            </div>
        </el-header>
        <el-main>
            <el-tabs v-model="activeName" @tab-click="handleClick">
                <el-tab-pane label="我提出的问题" name="first">
                    <el-table :data="askedQuestions" style="width: 100%">
                        <el-table-column prop="date" label="日期" style="width: 100%">
                        </el-table-column>
                    </el-table>
                </el-tab-pane>
                <el-tab-pane label="我回答的问题" name="second">
                    <el-table :data="answeredQuestions" style="width: 100%">
                        <el-table-column prop="date" label="日期" style="width: 100%">
                        </el-table-column>
                    </el-table>
                </el-tab-pane>
            </el-tabs>
        </el-main>
    </el-container>
</template>

<script>
export default {
    data() {
        return {
            nickName: "测试昵称",
            email: "1234567@qq.com",

            activeName: "first",

            askedQuestions: [{
                date: '提问题1',
            }, {
                date: '提问题2',
            }, {
                date: '提问题3',
            }, {
                date: '提问题4',
            }],
            answeredQuestions: [{
                date: '答问题1',
            }, {
                date: '答问题2',
            }, {
                date: '答问题3',
            }, {
                date: '答问题4',
            }]
        }
    },
    methods: {
        goBack() {
            this.$router.replace('/questionList');
        },
    },
    mounted() {
        // this.getQuestionDetail();
    }
}
</script>

<style scoped>
.el-page-header {
    margin: 20px;
}

.header {
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
    padding-top: 1px;
}

.el-tag {
    margin: 10px;
}

p {
    margin: 10px;
}
</style>
