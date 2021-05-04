<template>
    <el-container style="height: 100%">
        <el-page-header @back="goBack" content="问题详情">
        </el-page-header>
        <el-header style="height: auto">
            <div class="header">
                <h1 align="center">{{ title }}</h1>
                <p>{{ detail }}</p>
                <p class="creator" align="right">{{ creatorName }}</p>
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
                    <el-button class="submit" type="primary" @click="submitAnswer">提交回答</el-button>
                </div>
            </el-collapse-transition>
        </el-main>
        <el-main class="existAnswer">
            <div>
                <div v-for="answer in answers" class="userAnswer">
                    <p class="answerContent">{{ answer.content }}</p>
                    <p class="creator" align="right">{{ answer.creatorName}}</p>
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
            creatorName: '',
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
            _this.$axios.get("https://aiape.snowphoenix.design/api/test/questions/question?qid=" + id)
                .then(async function (response) {
                    _this.$data.title = response.data.question.title;
                    _this.$data.detail = response.data.question.remarks;
                    _this.$data.creator = response.data.question.creater;
                    // _this.$data.creatorName = await _this.getUserName(_this.$data.creator);
                    _this.$data.creatorName = 'test';
                    _this.$data.date = response.data.question.createTime;
                    _this.$data.tags = response.data.question.tags;
                    let aidList = response.data.question.answers;
                    let best = response.data.best;
                    for (let aid of aidList) {
                        _this.$axios.get(_this.BASE_URL + "/api/questions/answer?aid=" + aid)
                            .then(async function (response) {
                                let answer = response.data.answer;
                                let name = await _this.getUserName(response.data.answer.creator);
                                answer['creatorName'] = name;
                                if (best === aid) {
                                    _this.$data.answers.splice(0, 0, answer);
                                } else {
                                    _this.$data.answers.push(answer);
                                }
                            })
                            .catch(function (error) {
                                console.log(error);
                            })
                    }

                })
                .catch(function (error) {
                    alert('error!');
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
        },
        submitAnswer() {
            let answer = this.myAnswer;
            let _this = this;
            this.$axios.post(_this.BASE_URL + '/api/questions/add_answer', {
                qid: _this.$store.state.questionID,
                content: answer
            }, {
                headers: {
                    Authorization : 'Bearer ' + _this.$store.state.token,
                    type : 'application/json;charset=utf-8'
                }
            })
            .then(function (response) {
                _this.myAnswer = '';
                if (response.data.status === 'success') {
                    _this.$store.commit('addAImessage', '感谢你的回答!');
                }
                else {
                    _this.$store.commit('addAImessage', '你已经回答过这个问题啦!');
                }
            })
        },
        async getUserName(uid) {
            let _this = this;
            let name = '';
            await this.$axios.get(this.BASE_URL + '/api/user/public_info?uid=' + uid)
            .then(function (response) {
                name = response.data.name;
            })
            return name;
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
