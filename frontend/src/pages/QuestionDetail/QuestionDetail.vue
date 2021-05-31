<template>
    <el-container class="shell">
        <el-container class="list">
            <el-main class="question-detail" style="z-index: 1">
                <div class="user">
                    <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                               size="small" style="margin-right: 10px"></el-avatar>
                    {{ creatorName }}
                </div>
                <h1>{{ title }}</h1>
                <mavon-editor :style='"max-height:" + maxHeight' class="question" v-model="detail" ref=md
                              :subfield="false" defaultOpen="preview"
                              :toolbarsFlag="false" :editable="false"
                              :scrollStyle="false" :box-shadow="false">
                </mavon-editor>
                <el-button style="padding-bottom: 0" type="text" class="show-all" icon="el-icon-arrow-down" @click="showAll" v-if="showAllState === false">展开</el-button>
                <el-button style="padding-bottom: 0" type="text" class="show-all" icon="el-icon-arrow-up" @click="showAll" v-else>收起</el-button>
                <div class="other-info">
                    <div class="tags">
                        <el-tag v-for="(tid, tName) in tags" :key="tid">{{ tName }}</el-tag>
                    </div>
                    <div class="recommend-time">
                        <el-button style="margin-right: 20px" icon="el-icon-edit" size="mini" circle
                                   @click="answerAreaMove"></el-button>
                        <el-button class="recommend" type="primary" icon="el-icon-thumb">推荐</el-button>
                        <span>{{ date }}</span>
                    </div>
                </div>
            </el-main>
            <el-collapse-transition>
                <div v-show="showAnswerArea">
                    <mavon-editor class="editor" :toolbars="toolbars" v-model="myAnswer" ref=md
                                  :subfield="prop.subfield" :defaultOpen="prop.defaultOpen"
                                  :toolbarsFlag="prop.toolbarsFlag" :editable="prop.editable"
                                  :scrollStyle="prop.scrollStyle" :boxShadow="prop.boxShadow"
                                  placeholder="编辑你的回答...">
                    </mavon-editor>
                </div>
            </el-collapse-transition>
            <el-main class="answers">
                <div class="answer" v-for="answer in answers" :key="answer.id">
                    <div class="user">
                        <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                                   size="small" style="margin-right: 10px"></el-avatar>
                        {{ answer.creatorName }}
                    </div>
                    <mavon-editor class="content" ref=md v-model="answer.content"
                                  :subfield="false" defaultOpen="preview"
                                  :toolbarsFlag="false" :editable="false"
                                  :scrollStyle="false" :box-shadow="false">
                    </mavon-editor>
                    <div style="display: flex; justify-content: flex-end; align-items: center">
                        <el-button class="recommend" type="primary" icon="el-icon-thumb">推荐</el-button>
                        <span>{{ answer.createTime }}</span>
                    </div>
                </div>
            </el-main>
        </el-container>
        <DetailSideBar/>
    </el-container>
</template>

<script>
import MarkdownItVue from 'markdown-it-vue'
import 'markdown-it-vue/dist/markdown-it-vue.css'
import DetailSideBar from "./DetailSideBar";

export default {
    components: {
        MarkdownItVue,
        DetailSideBar
    },
    metaInfo: {
        meta: [
            {name: 'referrer', content: 'no-referrer'},
        ]
    },
    data() {
        return {
            title: '',
            detail: '',
            tags: ['Windows', 'C', '环境'],
            creatorName: '',
            date: '',
            answers: [],
            myAnswer: '',
            showAnswerArea: false,
            toolbars: {
                bold: true, // 粗体
                italic: true, // 斜体
                header: true, // 标题
                underline: true, // 下划线
                strikethrough: false, // 中划线
                mark: false, // 标记
                superscript: true, // 上角标
                subscript: true, // 下角标
                quote: false, // 引用
                ol: true, // 有序列表
                ul: true, // 无序列表
                link: false, // 链接
                imagelink: false, // 图片链接
                code: false, // code
                table: false, // 表格
                fullscreen: true, // 全屏编辑
                readmodel: false, // 沉浸式阅读
                htmlcode: false, // 展示html源码
                help: true, // 帮助
                /* 1.3.5 */
                undo: true, // 上一步
                redo: true, // 下一步
                trash: true, // 清空
                save: false, // 保存（触发events中的save事件）
                /* 1.4.2 */
                navigation: false, // 导航目录
                /* 2.1.8 */
                alignleft: true, // 左对齐
                aligncenter: true, // 居中
                alignright: true, // 右对齐
                /* 2.2.1 */
                subfield: false, // 单双栏模式
                preview: true // 预览
            },
            showAllState: false,
            maxHeight: '15vh'
        }
    },
    methods: {
        goBack() {
            this.$router.replace('/questionList');
        },
        getQuestionDetail() {
            let _this = this;
            let id = this.$store.state.questionID;
            _this.$axios.get(_this.BASE_URL + "/api/questions/question?qid=" + id)
                .then(async function (response) {
                    _this.title = response.data.question.title;
                    _this.detail = response.data.question.remarks;
                    let creatorId = response.data.question.creator;
                    await _this.$axios.get(_this.BASE_URL + '/api/user/public_info?uid=' + creatorId)
                    .then(function (response) {
                        _this.creatorName = response.data.name;
                    })
                    _this.$data.date = response.data.question.createTime;
                    _this.$data.tags = response.data.question.tags;
                    let aidList = response.data.question.answers;
                    for (let aid of aidList) {
                        _this.$axios.get(_this.BASE_URL + "/api/questions/answer?aid=" + aid)
                            .then(async function (response) {
                                console.log(response);
                                let answer = response.data.answer;
                                let id = response.data.answer.creator;
                                await _this.$axios.get(_this.BASE_URL + '/api/user/public_info?uid=' + id)
                                    .then(function (response) {
                                        answer.creatorName = response.data.name;
                                    })
                                answer['id'] = parseInt(response.data.message[response.data.message.indexOf('=') + 1]);
                                // if (best === aid) {
                                //     _this.$data.answers.splice(0, 0, answer);
                                // } else {
                                _this.answers.push(answer);
                                // }
                                // _this.answers.sort((a, b) => {
                                //     if (a['id'] === best) {
                                //         return -1;
                                //     }
                                //     if (b['id'] === best) {
                                //         return 1;
                                //     }
                                //     return b['id'] - a['id'];
                                // });
                            })
                            .catch(function (error) {
                                console.log(error);
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
        },
        submitAnswer() {
            let answer = this.myAnswer;
            let _this = this;
            this.$axios.post(_this.BASE_URL + '/api/questions/add_answer', {
                qid: _this.$store.state.questionID,
                content: answer
            }, {
                headers: {
                    Authorization: 'Bearer ' + _this.$store.state.token,
                    type: 'application/json;charset=utf-8'
                }
            })
                .then(function (response) {
                    _this.myAnswer = '';
                    console.log(response);
                    if (response.data.status === 'success') {
                        _this.$store.commit('addAImessage', {
                            id: 2,
                            content: '感谢你的回答!',
                            prompts: [],
                            promptValid: false
                        });
                        _this.getQuestionDetail();
                        location.reload();
                    } else {
                        _this.$store.commit('addAImessage', {
                            id: 2,
                            content: '你已经回答过这个问题啦!',
                            prompts: [],
                            promptValid: false
                        });
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
        },
        showAll() {
            if (!this.showAllState) {
                this.showAllState = true;
                this.maxHeight = '55vh';
            }
            else {
                this.showAllState = false;
                this.maxHeight = '15vh';
            }
        }
    },
    mounted() {
        this.getQuestionDetail();
    },
    computed: {
        prop() {
            return {
                subfield: false,// 单双栏模式
                defaultOpen: 'edit',//edit： 默认展示编辑区域 ， preview： 默认展示预览区域
                editable: true,
                toolbarsFlag: true,
                scrollStyle: false,
                boxShadow: true//边框
            };
        }
    }
}
</script>

<style scoped>
.shell {
    position: absolute;
    left: 5vw;
    top: 0;
    width: 95vw;
    height: 100vh;
    padding-left: 100px;
    padding-right: 100px;
    background-color: rgba(246, 246, 246, 1);
}

.list {
    flex-direction: column;
    border-radius: 2px;
    height: 95vh;
    align-items: stretch;
    margin-right: 5px;
    overflow: scroll;
}

.el-header {
    padding-top: 5px;
    font-size: 30px;
    box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
    background-color: white;
    align-self: stretch;
    justify-content: center;
}

.question-detail {
    flex-grow: 0;
    background-color: white;
    box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
    align-items: stretch;
    flex-direction: column;
    overflow: scroll;
    padding: 10px;
    flex-shrink: 0;
}

.question-detail * {
    display: flex;
}

h1 {
    margin-left: 30px;
}

.question {
    min-height: 15vh;
    border: 0;
}

.v-show-content {
    background-color: white !important;
}

.other-info {
    display: flex;
    margin-top: 10px;
    flex-direction: row;
    justify-content: space-between;
    margin-bottom: 10px;
}

.el-tag {
    margin-left: 10px;
    margin-top: 5px;
}

i {
    cursor: pointer;
    color: #409eff;
    font-size: 20px;
}

.recommend-user-time {
    display: flex;
    flex-direction: row;
}

/*.show-answer {*/
/*    font-size: 10px;*/
/*    height: 10px;*/
/*    width: 10px;*/
/*    line-height: 10px;*/
/*    margin-right: 10px;*/
/*    text-align: center;*/
/*}*/

.recommend {
    height: 20px;
    font-size: 10px;
    line-height: 20px;
    background-color: rgb(255, 255, 255);
    border-color: rgb(255, 255, 255);
    color: #966dff;
    padding: 3px 3px;
    margin-right: 20px;
    align-items: center;
}


.user-time {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    margin-right: 10px;
}

.markdown-body {
    border-radius: 0;
}

.editor {
    margin-top: 10px;
}

.answers {
    margin-top: 10px;
    background-color: white;
    box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
    flex-direction: column;
}

.content {
    border: 0;
    min-height: 0;
}

.answer {
    border-bottom: 1px solid lightgrey;
    padding: 10px;
}

.answer-user-time {
    margin-bottom: 10px;
}

.user {
    display: flex;
    align-self: flex-start;
    flex-direction: row;
    align-items: center;
}

.other-info {
    justify-content: space-between;
    align-items: center;
    margin-left: 25px;
}

.recommend-time {
    flex-direction: row;
    align-items: center;
}

.el-tag {
    height: 25px;
    line-height: 23px;
    font-size: 12px;
    margin-left: 5px;
    margin-bottom: 5px;
}

.show-all {
    align-self: flex-end;
    color: black;
}

.show-all:hover {
    color: #409eff;
}
</style>
