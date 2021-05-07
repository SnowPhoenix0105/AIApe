<template>
    <el-container style="height: 100%">
        <el-page-header @back="goBack" content="个人中心">
        </el-page-header>
        <el-header style="height: auto">
            <h1 align="center">基本信息</h1>
            <div class="header">
                <p>我的id：{{ uid }}</p>
                <p>我的昵称：{{ nickName }}</p>
                <p>我的邮箱：{{ email }}</p>
            </div>
        </el-header>
        <el-main>
            <el-tabs v-model="activeName">
                <el-tab-pane label="我提出的问题" name="first">
                    <!--                    <el-table :data="askedQuestions" style="width: 100%">-->
                    <!--                        <el-table-column prop="date" label="日期" style="width: 100%">-->
                    <!--                        </el-table-column>-->
                    <!--                    </el-table>-->
                    <el-table
                        :data="questions"
                        style="width: 100%"
                        :header-cell-style="{textAlign: 'center'}"
                        :cell-style="{ textAlign: 'center' }">
                        <el-table-column
                            prop="id"
                            label="编号"
                            width="180">
                        </el-table-column>
                        <el-table-column
                            label="问题">
                            <template slot-scope="scope">
                                <el-popover trigger="hover" placement="top">
                                    <p>编号: {{ scope.row.id }}</p>
                                    <p>问题: {{ scope.row.title }}</p>
                                    <el-link @click="goToDetail(scope.row.id)" slot="reference">{{
                                            scope.row.title
                                        }}
                                    </el-link>
                                </el-popover>
                            </template>
                        </el-table-column>
                    </el-table>
                </el-tab-pane>
                <el-tab-pane label="我回答的问题" name="second">
                    <el-table
                        :data="answers"
                        style="width: 100%"
                        :header-cell-style="{textAlign: 'center'}"
                        :cell-style="{ textAlign: 'center' }">
                        <el-table-column
                            prop="aid"
                            label="编号"
                            width="180">
                        </el-table-column>
                        <el-table-column
                            label="回答"
                            prop="content"
                        >
                        </el-table-column>
                        <el-table-column
                            label="编辑"
                            width="180">
                            <template slot-scope="scope">
                                <el-link @click="removeAnswer(scope.row.aid)" slot="reference">
                                    删除
                                </el-link>
                            </template>
                        </el-table-column>
                    </el-table>
                </el-tab-pane>
            </el-tabs>
        </el-main>
    </el-container>
</template>

<script>
import store from "../../vuex/store";

export default {
    data() {
        return {
            uid: "测试id",
            nickName: "测试昵称",
            email: "测试邮箱",

            activeName: "first",

            questions: [],
            answers: [],
        }
    },
    methods: {
        goBack() {
            this.$router.replace('/questionList');
        },
        removeAnswer(aid) {
            // alert('删除回答' + aid);
            let _this = this;
            let token = store.state.token;
            _this.$axios.delete(_this.BASE_URL + '/api/questions/delete_answer', {
                data: {aid: aid},
                headers: {
                    Authorization: 'Bearer ' + token,
                },
            })
                .then(function (response) {
                    console.log(response);
                    _this.answers = [];
                    _this.getAnswers();
                })
        },
        getAnswers() {
            let _this = this;
            let token = store.state.token;
            _this.$axios.get(this.BASE_URL + '/api/user/answers', {
                headers: {
                    Authorization: 'Bearer ' + token,
                },
            })
                .then(function (response) {
                    // console.log('成功获得回答序号');
                    // console.log(response);
                    // console.log(response.data.answers);
                    let answerIdList = response.data.answers;
                    for (let aid of answerIdList) {
                        _this.$axios.get(_this.BASE_URL + '/api/questions/answer?aid=' + aid)
                            .then(function (res) {
                                // console.log("问题详细信息：");
                                // console.log(res);
                                _this.$data.answers.push({
                                    'aid': aid,
                                    'content': res.data.answer.content
                                });
                            })
                    }
                })
        },
        getQuestions() {
            let _this = this;
            _this.$axios.post(_this.BASE_URL + '/api/test/questions/questionlist', {
                number: 5
            })
                .then(function (response) {
                    let questionIdList = response.data;
                    questionIdList.sort();
                    for (let qid of questionIdList) {
                        _this.$axios.get('https://aiape.snowphoenix.design/api/test/questions/question?qid=' + qid)
                            .then(function (response) {
                                _this.$data.questions.push({
                                    'id': qid,
                                    'title': response.data.question.title
                                });
                            });
                    }
                })
        },
        goToDetail(qid) {
            this.$router.replace('questionDetail');
            this.$store.commit('setQuestionID', qid);
        },
    },
    mounted() {
        this.getQuestions();
        this.getAnswers();
        let _this = this;
        // var token = this.GLOBAL.token;
        let token = store.state.token;
        // console.log(token);
        console.log('获取到的token是' + token);
        this.$axios.get(this.BASE_URL + '/api/user/internal_info', {
            headers: {
                Authorization: 'Bearer ' + token,
            },
        })
            .then(function (response) {
                console.log('成功获得内部信息');
                console.log(response);
                _this.nickName = response.data.name;
                _this.email = response.data.email;
                _this.uid = response.data.uid;

                _this.$axios.get(_this.BASE_URL + '/api/user/answers', {
                    headers: {
                        Authorization: 'Bearer ' + token,
                    },
                })
                    .then(function (response) {
                        console.log('成功获得回答列表');
                        console.log(response);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            })
            .catch(function (error) {
                console.log(error);
            });
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
