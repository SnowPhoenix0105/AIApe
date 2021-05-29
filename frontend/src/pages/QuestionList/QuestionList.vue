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
            <div style="height: auto; overflow: auto; width: 51vw" ref="scroll-body" @scroll="loadMore">
                <el-main class="question-list">
                    <div class="question-body" v-for="question in questionList" :key="question.id">
                        <div class="recommend">
                            <i class="el-icon-caret-top"></i>
                            <span>推荐{{ question.recommend }}</span>
                        </div>
                        <div class="content">
                            {{ question.content }}
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
            </div>
        </el-container>
        <ListSideBar/>
    </el-container>
</template>

<script>
import ListSideBar from "./ListSideBar";

export default {
    components: {
        ListSideBar
    },
    data() {
        return {
            questionList: [],
            last_index: 0,
            selectedTag: [],
            tagState: {},
            showTag: true,
            isAdmin: false,
            showResearchInput: false,
            select: 0,
            loading: false,
            no_more: false
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
        getQuestions(pt) {
            // console.log("getQuestions!");
            let _this = this;

            let post_data = {
                number: 64,
                tags: _this.selectedTag,
            }
            if (pt > 0) {
                post_data['pt'] = pt;
            }
            _this.$axios.post(_this.BASE_URL + '/api/questions/questionlist', post_data)
                .then(async function (response) {
                    let questionIdList = response.data;
                    _this.last_index = questionIdList[questionIdList.length - 1];
                    if (questionIdList.length < 64) {
                        _this.no_more = true;
                    }
                    // let questions = [];
                    for (let qid of questionIdList) {
                        await _this.$axios.get(_this.BASE_URL + '/api/questions/question?qid=' + qid)
                            .then(function (response) {
                                _this.questionList.push({
                                    id: qid,
                                    title: response.data.question.title,
                                    content: response.data.question.remarks
                                });
                                // _this.$data.questionList = questions;
                                // questions.sort((a, b) => a.id - b.id);
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
        },
        loadMore() {

            let e = this.$refs['scroll-body'];
            if (e.scrollTop + e.offsetHeight > e.scrollHeight - 1 && !this.loading && !this.no_more) {
                this.loading = true;
                this.getQuestions(this.last_index);
                this.loading = false;
            }
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
    background-color: rgba(246, 246, 246, 0.5);
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
    padding-top: 5px;
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
    height: 110px;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-box-orient: vertical;
}

i {
    color: #409eff;
    font-size: 60px;
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

.detail {
    text-overflow: ellipsis;
}
</style>
