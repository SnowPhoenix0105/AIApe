<template>
    <el-container>
        <el-header height=5vh>
            AIApe
        </el-header>
        <el-container class="list">
            <el-main class="selector">
                <el-button type="text" :class="{'unselected': select === 'hot'}" @click="handleSelect('new')">最新
                </el-button>
                <el-button type="text" :class="{'unselected': select === 'new'}" @click="handleSelect('hot')">热门
                </el-button>
            </el-main>
            <div style="height: auto; overflow: auto; width: 100vw" ref="scroll-body" id="scroll-body"
                 @scroll="loadMore">
                <el-main class="question-list">
                    <div class="question-body" v-for="question in questionList" :key="question.id"
                         v-if="select==='new'">
                        <div class="user">
                            <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                                       size="small" style="margin-right: 10px"></el-avatar>
                            {{ question.creator }}
                        </div>
                        <el-link class='title' @click="goToDetail(question.id)" :underline="false">
                            {{ question.title }}
                        </el-link>

                        <!--                        <p class="content">-->
                        <!--                            {{ question.content }}-->
                        <!--                        </p>-->
                        <div class="other-info">
                            <div class="tags">
                                <el-tag v-for="(tid, tName) in question.tags" :key="tid">{{ tName }}</el-tag>
                            </div>
                            <div class="recommend-time">
                                <el-button class="recommend" type="primary" icon="el-icon-thumb">推荐</el-button>
                                <span>{{ question.date }}</span>
                            </div>
                        </div>
                    </div>
                    <div class="question-body" v-for="question in hots" :key="question.id" v-else>
                        <div class="user">
                            <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                                       size="small" style="margin-right: 10px"></el-avatar>
                            {{ question.creator }}
                        </div>
                        <el-link class='title' @click="goToDetail(question.id)" :underline="false">
                            {{ question.title }}
                        </el-link>
                        <div class="content">
                            {{ question.content }}
                        </div>
                        <div class="other-info">
                            <div class="tags">
                                <el-tag v-for="(tid, tName) in question.tags" :key="tid">{{ tName }}</el-tag>
                            </div>
                            <div class="recommend-time">
                                <el-button class="recommend" type="primary" icon="el-icon-thumb">推荐</el-button>
                                <span>{{ question.date }}</span>
                            </div>
                        </div>
                    </div>
                </el-main>
            </div>
        </el-container>
    </el-container>
</template>

<script>

import 'markdown-it-vue/dist/markdown-it-vue.css'

export default {
    name: "MobileQuestionList",
    data() {
        return {
            hots: [],
            questionList: [],
            last_index: 0,
            selectedTag: [],
            tagState: {},
            showTag: true,
            isAdmin: false,
            showResearchInput: false,
            select: 'new',
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
        handleSelect(selector) {
            if (selector === this.select) {
                return;
            }
            if (selector === 'new') {
                this.select = 'new';
            } else {
                this.select = 'hot';
            }
        },
        getQuestions(pt) {
            // console.log("getQuestions!");
            let _this = this;

            let post_data = {
                number: 10,
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
                    for (let qid of questionIdList) {
                        await _this.$axios.get(_this.BASE_URL + '/api/questions/question?qid=' + qid)
                            .then(async function (response) {
                                let question = {
                                    id: qid,
                                    title: response.data.question.title,
                                    content: response.data.question.remarks,
                                    tags: response.data.question.tags,
                                    date: response.data.question.createTime
                                };
                                let uid = response.data.question.creator;
                                await _this.$axios.get(_this.BASE_URL + '/api/user/public_info?uid=' + uid)
                                    .then(function (response) {
                                        question.creator = response.data.name;
                                    })
                                _this.questionList.push(question);
                            });

                    }
                })
                .catch(function (error) {

                });

        },
        goToDetail(qid) {
            this.$store.commit('setQuestionID', qid);
            // this.$changePage(3);
            this.$store.state.mobileStatus = 'questionDetail';
        },
        goToPersonalCenter() {
            // this.$router.replace('PersonalCenter');
        },

        gotoAdministration() {
            // this.$router.replace('/administration');
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
        async loadMore(e) {

            if (e.srcElement.scrollTop + e.srcElement.clientHeight == e.srcElement.scrollHeight) {
            // if (true) {
                this.loading = true;
                await this.getQuestions(this.last_index);
                this.loading = false;
                // alert('here')
            }
        },
        getHot() {

        }
    },
}
</script>

<style scoped>
/*.shell {*/
/*    position: relative;*/
/*    left: 0;*/
/*    top: 0;*/
/*    width: 100vw;*/
/*    height: 90vh;*/
/*    padding-left: 0;*/
/*    padding-right: 0;*/
/*    background-color: rgba(246, 246, 246, 0.5);*/
/*}*/

.list {
    flex-direction: column;
    box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
    border-radius: 2px;
    height: 75vh;
    width: 100vw;
    background-color: white;
    margin-right: 0;
}

.el-header {
    border-bottom: 1px solid #eaecf1;
    width: 100vw;
    align-items: center;
    padding: 5%;
    /*height: 5vh;*/
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

.recommand:hover {
    color: rgb(39, 214, 214);
}

.question-list {
    align-self: stretch;
    flex-direction: column;
}

.user {
    align-self: flex-start;
    flex-direction: row;
    align-items: center;
}

.el-link {
    justify-content: flex-start;
    font-size: 20px;
    font-weight: bold;
    color: black;
    flex-grow: 0;
}

.question-list * {
    display: flex;
}

.question-body {
    flex-direction: column;
    padding: 10px;
    flex: 1 0 110px;
    border-bottom: 1px solid lightgrey;
}

.content {
    flex-grow: 2;
    justify-content: flex-start;
    align-items: flex-start;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
    line-height: 20px;
    height: 60px;
    margin-top: 10px;
    margin-left: 30px;
    margin-right: 20px;
}

i {
    color: #409eff;
    font-size: 30px;
}

i:hover {
    color: #6dfff3;
}

.title {
    font-size: 20px;
    font-weight: bold;
    margin-top: 10px;
    margin-left: 30px;
    /*white-space: nowrap;*/
    /*overflow: hidden;*/
    /*text-overflow: ellipsis;*/
    /*display: block;*/
    margin-right: 20px;
    overflow: hidden;
}

.tags {
    flex-direction: row;
}

.other-info {
    justify-content: space-between;
    align-items: center;
    margin-left: 25px;
    margin-right: 20px;
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

.detail {
    text-overflow: ellipsis;
}

</style>

