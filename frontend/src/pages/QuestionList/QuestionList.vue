<template>
    <el-container class="shell">
        <el-container class="list">
            <el-header>
                AIApe
            </el-header>
            <el-main class="selector">
                <el-button type="text" :class="{'unselected': select === 1}">最新</el-button>
                <el-button type="text" :class="{'unselected': select === 0}">热门</el-button>
            </el-main>
            <el-main class="question-list">
                <div class="question-body" v-for="question in questionList" :key="question.id">
                    <div class="recommend">
                        <i class="el-icon-circle-plus-outline"></i>
                        <span>推荐{{ question.recommend }}</span>
                    </div>
                    <div class="content">
                        <span class="title">{{ question.title }}</span>
                        <span class="detail">{{ question.detail }}</span>
                    </div>
                    <div class="other-info">
                        <div class="tags">
                            <el-tag v-for="tag in question.tags" :key="tag">{{ tag }}</el-tag>
                        </div>
                        <div class="user-time">
                            <span>{{ question.user }}</span>
                            <span>{{ question.date }}</span>
                        </div>
                    </div>
                </div>
            </el-main>
        </el-container>
        <ListSideBar/>
    </el-container>
</template>

<script>
import MarkdownItVue from 'markdown-it-vue'
import 'markdown-it-vue/dist/markdown-it-vue.css'
import ListSideBar from "./ListSideBar";

export default {
    components: {
        MarkdownItVue,
        ListSideBar
    },
    data() {
        return {
            questionList: [{
                id: 1, title: '这是问题标题', detail: '这是问题详情',
                recommend: 776, user: 'william', date: '2021-5-24', tags: ['Linux', 'Python', 'Windows']
            },{
                id: 1, title: '这是问题标题', detail: '这是问题详情',
                recommend: 777, user: 'william', date: '2021-5-24', tags: ['Linux', 'Python', 'Windows']
            },{
                id: 1, title: '这是问题标题', detail: '这是问题详情',
                recommend: 777, user: 'william', date: '2021-5-24', tags: ['Linux', 'Python', 'Windows']
            },{
                id: 1, title: '这是问题标题', detail: '这是问题详情',
                recommend: 777, user: 'william', date: '2021-5-24', tags: ['Linux', 'Python', 'Windows']
            },{
                id: 1, title: '这是问题标题', detail: '这是问题详情',
                recommend: 777, user: 'william', date: '2021-5-24', tags: ['Linux', 'Python', 'Windows']
            },{
                id: 1, title: '这是问题标题', detail: '这是问题详情',
                recommend: 777, user: 'william', date: '2021-5-24', tags: ['Linux', 'Python', 'Windows']
            },{
                id: 1, title: '这是问题标题', detail: '这是问题详情',
                recommend: 777, user: 'william', date: '2021-5-24', tags: ['Linux', 'Python', 'Windows']
            },{
                id: 1, title: '这是问题标题', detail: '这是问题详情',
                recommend: 777, user: 'william', date: '2021-5-24', tags: ['Linux', 'Python', 'Windows']
            },],
            selectedTag: [],
            tagState: {},
            showTag: true,
            isAdmin: false,
            showResearchInput: false,
            select: 0
        }
    },
    mounted() {
        this.getQuestions();
        this.initTagState();
        this.isAdmin = (this.$store.state.auth === 2);
    },
    methods: {
        handleSelect() {

        },
        getQuestions() {
            let _this = this;

            _this.$axios.post(_this.BASE_URL + '/api/questions/questionlist', {
                number: 16,
                tags: _this.$data.selectedTag

            })
                .then(function (response) {
                    let questionIdList = response.data;
                    questionIdList.sort();

                    let questions = [];
                    for (let qid of questionIdList) {
                        _this.$axios.get(_this.BASE_URL + '/api/questions/question?qid=' + qid)
                            .then(function (response) {
                                questions.push({
                                    id: qid,
                                    title: response.data.question.title,
                                    content: response.data.question.remarks
                                });
                                _this.$data.questions = questions;
                                questions.sort((a, b) => a.id - b.id);
                            });
                    }

                })
                .catch(function (error) {
                    _this.questions = [];
                });

        },
        goToDetail(qid) {
            this.$router.replace('questionDetail');
            this.$store.commit('setQuestionID', qid);
        },
        goToPersonalCenter() {
            this.$router.replace('PersonalCenter');
        },

        gotoAdministration() {
            this.$router.replace('/administration');
        },
        handleCommand(command) {
            if (command === 'personalCenter') {
                this.goToPersonalCenter();
            } else if (command === 'administration') {
                this.gotoAdministration();
            }
        },

        initTagState() {
            let tagList = this.$store.state.tagList;
            for (let tagName in tagList) {
                this.$data.tagState[tagList[tagName]] = false;
            }
        },
        tagClick(tid) {
            if (!this.tagState[tid]) {
                this.tagState[tid] = true;
                this.selectedTag.push(tid);
            } else {
                this.tagState[tid] = false;
                let index = this.selectedTag.indexOf(tid);
                this.selectedTag.splice(index, 1);
            }
            this.showTag = false;
            this.$nextTick(function () {
                this.showTag = true;
            })
            this.getQuestions();
        },
        close() {
            this.$store.state.show.questionList = false;
        },
        gotoTop() {
            this.$store.state.maxZIndex += 1
            this.zIndex = this.$store.state.maxZIndex;
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
    box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
    border-radius: 2px;
    height: 95vh;
    background-color: white;
    margin-right: 5px;
}

.el-header {
    padding-top: 10px;
    font-size: 30px;
}

.selector {
    flex: none;
    align-self: stretch;
    padding-left: 10px;
    border-bottom: 1px solid lightgrey;
}

.unselected {
    color: black;
}

.el-button {
    font-size: 20px;
}

.el-button:hover {
    color: #409eff;
}

.question-list {
    align-self: stretch;
    flex-direction: column;
}

.question-body {
    flex-direction: row;
    border-bottom: 1px solid lightgrey;
}

.question-list * {
    display: flex;
}

.question-body {
    flex-direction: row;
    padding: 20px;
    flex: 1 0 110px;
}

.question-body > * {
    flex-direction: column;
    align-items: center;
    justify-content: center;
}

.content {
    flex-grow: 2;
    justify-content: flex-start;
    align-items: flex-start;
    margin-left: 20px;
}

i {
    color: #409eff;
    font-size: 30px;
}

.i:hover{
    color: #6dfff3;
}

.title {
    font-size: 20px;
    font-weight: bold;
}

.tags {
    flex-direction: row;
    flex-wrap: wrap;
}

.other-info {
    justify-content: space-around;
    align-items: flex-end;
    flex: 0 1 125px;
}

.user-time {
    flex-direction: column;
    align-items: flex-end;
}

.el-tag {
    height: 25px;
    line-height: 23px;
    font-size: 12px;
    margin-left: 5px;
    margin-bottom: 5px;
}

.recommend {
    flex-grow: 0;
}
</style>
