<template>
    <el-container style="height: 100%">
        <el-page-header @back="goBack" content="问题详情">
        </el-page-header>
        <el-header style="height: auto">
            <div class="header">
                <h1 align="center">{{ title }}</h1>
                <p>{{ detail }}</p>
                <p class="creator" align="right">{{ creator }}</p>
                <p class="date" align="right">{{ date }}</p>
                <el-tag v-for="(tid, tag_name, index) in tags" :key="tid">{{ tag_name }}</el-tag>
                <el-button class="answer" :icon="icon" circle @click="answerAreaMove"></el-button>
            </div>
        </el-header>
        <el-main class="answerArea">
            <el-collapse-transition>
                <div v-show="showAnswerArea">
                    <el-input type="textarea" resize="none" :autosize="{ minRows: 6, maxRows: 6}"
                              v-model="myAnswer"></el-input>
                    <el-button class="submit" type="primary">提交回答</el-button>
                </div>
            </el-collapse-transition>
        </el-main>
        <el-main class="existAnswer">
            <div>
                <div v-for="answer in answers" class="userAnswer">
                    <p class="answerContent">{{ answer.content }}</p>
                    <p class="creator" align="right">{{ answer.creater }}</p>
                    <p class="date" align="right">{{ answer.createTime }}</p>
                </div>
            </div>
        </el-main>
    </el-container>
</template>

<script>
export default {
    data() {
        return {
            title: '',
            detail: '',
            tags: {},
            creator: '',
            date: '',
            answers: [],
            myAnswer: '',
            icon: 'el-icon-edit',
            showAnswerArea: false
        }
    },
    methods: {
        goBack() {
            this.$router.replace('/questionList');
        },
        getQuestionDetail() {
            let _this = this;
            let id = this.$store.state.questionID;
            console.log(this.$data.tags);
            _this.$axios.get("https://aiape.snowphoenix.design/api/test/questions/question?qid=" + id)
                .then(function (response) {
                    console.log(response);
                    _this.$data.title = response.data.question.title;
                    _this.$data.detail = response.data.question.remarks;
                    _this.$data.creator += response.data.question.creater;
                    _this.$data.date = response.data.question.createTime;
                    _this.$data.tags = response.data.question.tags;
                    let aidList = response.data.question.answers;
                    let best = response.data.best;
                    for (let aid of aidList) {
                        console.log(aid);
                        _this.$axios.get("https://aiape.snowphoenix.design/api/questions/answer?aid=" + aid)
                            .then(function (response) {
                                if (best === aid) {
                                    _this.$data.answers.splice(0, 0, response.data.answer);
                                } else {
                                    _this.$data.answers.push(response.data.answer);
                                }
                            })
                            .catch(function (error) {
                                console.log(error);
                                _this.$data.answers.push({
                                    content: "C语言xxxxxwdnm",
                                    creater: "huang",
                                    createTime: "xxxxxxxxxxxx"
                                })
                            })

                    }

                })
                .catch(function (error) {
                    console.log(error);
                });
        },
        answerAreaMove() {
            if (!this.$data.showAnswerArea) {
                this.$data.showAnswerArea = true;
                this.$data.icon = 'el-icon-arrow-up';
            } else {
                this.$data.showAnswerArea = false;
                this.$data.icon = 'el-icon-edit';
            }
        }
    },
    mounted() {
        this.getQuestionDetail();
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

.answer {
    float: right;
    height: 32px;
    width: 32px;
    margin: 10px;
    padding: 0;
}

.el-input {
    margin: 200px;
}

.el-textarea {
    font-family: "Microsoft YaHei", serif;
    font-size: 18px;
}

.submit {
    margin-top: 10px;
    float: right;
}

.existAnswer {
    overflow: scroll;
    flex: 1;
    padding-top: 0;
    padding-bottom: 0;
    margin-top: 10px;
}

.answerArea {
    flex: 0;
    overflow: visible;
    height: auto;
    padding-bottom: 0;
}

.el-header {
    display: flex;
    flex-flow: column wrap;
}

.answerContent {
    word-wrap: break-word;
}

.userAnswer {
    border-bottom: 2px solid #eaecf1;
}
</style>
