<template>
    <div class="shell" v-drag :style="'z-index:' + zIndex" @click="gotoTop">
        <el-container>
            <el-header style="height: auto">
                <i class="el-icon-circle-close" style="cursor: pointer" @click="close"></i>
                <span>问题列表</span>
                <i class="el-icon-search" style="cursor: pointer" @click="showResearchInput = !showResearchInput"></i>
                <el-collapse-transition>
                    <el-input v-show="showResearchInput"></el-input>
                </el-collapse-transition>
            </el-header>
            <el-main class="mode-selector">
                <el-menu style="text-align: center" default-active="1" mode="horizontal" @select="handleSelect">
                    <el-menu-item style="width: 40vw" index="1">热门</el-menu-item>
                    <el-menu-item style="width: 40vw" index="2">最新</el-menu-item>
                </el-menu>
            </el-main>
            <el-main class="tag-selector" v-if="showTag">
                <el-tag v-for="(tid, tag_name) in this.$store.state.tagList" :key="tid"
                        :effect="tagState[tid]? 'dark' : 'light'" @click="tagClick(tid)">{{ tag_name }}
                </el-tag>
            </el-main>
            <el-main class="table">
                <el-table
                    :data="questions"
                    style="width: 100%"
                    :header-cell-style="{textAlign: 'center'}"
                    :cell-style="{ textAlign: 'center' }">
                    <el-table-column prop="id" label="编号" width="180"></el-table-column>
                    <el-table-column label="问题">
                        <template slot-scope="scope">
                            <el-link @click="goToDetail(scope.row.id)" slot="reference">{{ scope.row.title }}</el-link>
                        </template>
                    </el-table-column>
                </el-table>
            </el-main>
        </el-container>
    </div>
</template>

<script>
import MarkdownItVue from 'markdown-it-vue'
import 'markdown-it-vue/dist/markdown-it-vue.css'

export default {
    components: {
        MarkdownItVue
    },
    data() {
        return {
            questions: [],
            selectedTag: [],
            tagState: {},
            showTag: true,
            isAdmin: false,
            showResearchInput: false,
            zIndex: this.$store.state.maxZIndex
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
    },
    directives: {
        drag(el) {
            el.onmousedown = function (e) {
                var disx = e.pageX - el.offsetLeft
                var disy = e.pageY - el.offsetTop
                document.onmousemove = function (e) {
                    el.style.left = e.pageX - disx + 'px'
                    el.style.top = e.pageY - disy + 'px'
                }
                document.onmouseup = function () {
                    document.onmousemove = document.onmouseup = null
                }
            }
        }
    }
}
</script>

<style scoped>
.shell {
    position: absolute;
    border: 1px solid lightgrey;
    left: 35px;
    top: 0;
    width: 80vw;
    height: 100vh;
    background-color: white;
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
    border-radius: 5px;
}

.el-header {
    /*border-bottom: 1px solid #eaecf1;*/
    align-items: center;
    width: 80vw;
    flex-direction: column;
}

.el-icon-circle-close {
    align-self: flex-end;
    color: lightgrey;
    margin: 2px;
}

.el-icon-search {
    align-self: flex-end;
    font-size: 20px;
    margin-right: 10px;
}

.el-input {
    width: 20vw;
    align-self: flex-end;
    margin: 10px;
}

.el-main {
    width: 80vw;
}

.tag-selector {
    align-items: center;
}

.el-tag {
    margin-top: 20px;
    margin-bottom: 20px;
    margin-left: 10px;
    cursor: pointer;
}

</style>
