<template>
    <div>
        <el-page-header @back="goBack" content="问题详情">
        </el-page-header>
        <div>
            <h2>用户管理</h2>
            <el-form ref="form" :model="userForm" label-width="100px">
                <el-form-item label="用户id">
                    <el-input v-model="userForm.uid" placeholder="输入用户id进行查询"></el-input>
                </el-form-item>
                <el-form-item label="用户昵称">
                    <el-input v-model="userForm.nickName"></el-input>
                </el-form-item>
                <el-form-item label="用户邮箱">
                    <el-input v-model="userForm.email"></el-input>
                </el-form-item>
                <el-form-item label="权限等级">
                    <el-input v-model="userForm.auth" placeholder="1 表示用户， 2 表示管理员"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="queryUserInfo">查询</el-button>
                </el-form-item>
                <br/>
                <el-form-item>
                    <el-button type="primary" @click="modifyUserInfo">保存其他信息</el-button>
                </el-form-item>
                <br/>
                <!--                <el-form-item>-->
                <!--                    <el-button type="primary" @click="modifyUserPassword">保存密码</el-button>-->
                <!--                </el-form-item>-->
                <!--                <br/>-->
                <!--                <el-form-item label="用户密码">-->
                <!--                    <el-input v-model="userForm.password"></el-input>-->
                <!--                </el-form-item>-->
            </el-form>
            <br/>
            <hr/>
        </div>
        <div>
            <h2>问题管理</h2>
            <el-form ref="form" :model="questionForm" label-width="100px">
                <el-form-item label="问题id">
                    <el-input v-model="questionForm.qid"></el-input>
                </el-form-item>
                <el-form-item label="问题题目">
                    <el-input v-model="questionForm.question"></el-input>
                </el-form-item>
                <el-form-item label="问题内容">
                    <el-input v-model="questionForm.remarks"></el-input>
                </el-form-item>
                <el-form-item label="问题回答编号">
                    <el-input v-model="questionForm.answers"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="queryQuestionInfo">查询问题</el-button>
                </el-form-item>
                <br/>
                <el-form-item>
                    <el-button type="primary" @click="">删除问题</el-button>
                </el-form-item>
                <br/>
                <el-form-item>
                    <el-button type="primary" @click="modifyQuestionInfo">保存修改</el-button>
                </el-form-item>
            </el-form>
            <br/>
            <hr/>
        </div>
        <div>
            <h2>回答管理</h2>
            <el-form ref="form" :model="answerForm" label-width="100px">
                <el-form-item label="回答id">
                    <el-input v-model="answerForm.aid"></el-input>
                </el-form-item>
                <el-form-item label="回答内容">
                    <el-input v-model="answerForm.content"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="queryAnswerInfo">查询回答</el-button>
                </el-form-item>
                <br/>
                <el-form-item>
                    <el-button type="primary" @click="">删除回答</el-button>
                </el-form-item>
                <br/>
                <el-form-item>
                    <el-button type="primary" @click="">保存修改</el-button>
                </el-form-item>
            </el-form>
            <br/>
            <hr/>
        </div>
        <div>
            <h2>标签管理</h2>
            <el-form ref="form" :model="tagForm" label-width="100px">
                <el-form-item label="标签id">
                    <el-input v-model="tagForm.tid"></el-input>
                </el-form-item>
                <el-form-item label="标签名">
                    <el-input v-model="tagForm.name"></el-input>
                </el-form-item>
                <el-form-item label="标签描述">
                    <el-input v-model="tagForm.desc"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="">删除标签</el-button>
                </el-form-item>
                <br/>
                <el-form-item>
                    <el-button type="primary" @click="">保存修改</el-button>
                </el-form-item>
            </el-form>
            <br/>
            <hr/>
        </div>
    </div>
</template>

<script>
import Chat from '../../components/Chat/Chat.vue'

export default {
    data() {
        return {
            userForm: {
                uid: '',
                nickName: '',
                email: '',
                auth: '',
                password: '',
            },
            questionForm: {
                qid: '',
                question: '',
                remarks: '',
                answers: [],
            },
            answerForm: {
                aid: '',
                content: '',
            },
            tagForm: {
                tid: '',
                name: '',
                desc: '',
            },
        }
    },
    methods: {
        goBack() {
            this.$router.replace('/questionList');
        },
        queryUserInfo() {
            let _this = this;
            let token = _this.$store.state.token;
            _this.$axios.get(_this.BASE_URL + '/api/user/full_info?uid=' + _this.userForm.uid, {
                headers: {
                    Authorization: 'Bearer ' + token,
                },
            })
                .then(function (response) {
                    console.log(response);
                    _this.userForm.nickName = response.data.name;
                    _this.userForm.email = response.data.email;
                    _this.userForm.auth = response.data.auth;
                });
        },
        modifyUserInfo() {
            let _this = this;
            let token = _this.$store.state.token;
            _this.$axios.put(_this.BASE_URL + '/api/user/modify', {
                uid: _this.userForm.uid,
                name: _this.userForm.nickName,
                auth: _this.userForm.auth,
            }, {
                headers: {
                    Authorization: 'Bearer ' + token,
                },
            })
                .then(function (response) {
                    console.log(response);
                })
        },
        // modifyUserPassword() {
        //     let _this = this;
        //     let token = _this.$store.state.token;
        //     _this.$axios.put(_this.BASE_URL + '/api/user/modify', {
        //         uid: _this.userForm.uid,
        //         password: _this.userForm.password,
        //     }, {
        //         headers: {
        //             Authorization: 'Bearer ' + token,
        //         },
        //     })
        //         .then(function (response) {
        //             console.log(response);
        //         }).catch(function (error) {
        //         console.log(error);
        //     })
        // }
        queryQuestionInfo() {
            let _this = this;
            let token = _this.$store.state.token;
            _this.$axios.get(_this.BASE_URL + '/api/questions/question?qid=' + _this.questionForm.qid, {
                headers: {
                    Authorization: 'Bearer ' + token,
                },
            })
                .then(function (response) {
                    console.log(response);
                    _this.questionForm.question = response.data.question.title;
                    _this.questionForm.remarks = response.data.question.remarks;
                    _this.questionForm.answers = response.data.question.answers;
                });
        },
        modifyQuestionInfo() {
            // alert('HERE!!!');
            let _this = this;
            alert(_this.questionForm.qid);
            alert(_this.questionForm.question);
            alert(_this.questionForm.remarks);
            let token = _this.$store.state.token;
            _this.$axios.put(_this.BASE_URL + '/api/questions/modify_question', {
                qid: _this.questionForm.qid,
                question: _this.questionForm.question,
                remarks: _this.questionForm.remarks
            }, {
                headers: {
                    Authorization: 'Bearer ' + token,
                },
            })
                .then(function (response) {
                    console.log(response);
                })
        },
        queryAnswerInfo() {
            let _this = this;
            let token = _this.$store.state.token;
            _this.$axios.get(_this.BASE_URL + '/api/questions/answer?aid=' + _this.answerForm.aid, {
                headers: {
                    Authorization: 'Bearer ' + token,
                },
            }).then(function (response) {
                console.log(response);
                _this.answerForm.content = response.data.answer.content;
            });
        }
    }
}
</script>

<style scoped>

.el-form {
    /*position: absolute;*/
    /*left: 35%;*/
    /*top: 35%;*/
}

.el-button {
    position: absolute;
    left: 35%;
}

.el-link {
    position: absolute;
    margin-top: 20px;
    left: 33%;
}
</style>
