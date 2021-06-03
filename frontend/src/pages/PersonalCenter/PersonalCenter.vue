<template>
    <el-container class="shell">
        <el-container class="list">
            <el-header>
                AIApe
            </el-header>
            <el-main class="selector">
                <el-button type="text" :class="{'unselected': select === 'answer'}" @click="handleSelect('question')">我的提问
                </el-button>
                <el-button type="text" :class="{'unselected': select === 'question'}" @click="handleSelect('answer')">我的回答
                </el-button>
            </el-main>
            <div style="height: auto; overflow: auto; width: 51vw" ref="scroll-body" id="scroll-body">
                <el-main class="question-list" v-if="select==='question'">
                    <div class="question-body" v-for="question in questions">
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
                                <el-button class="recommend" type="text"
                                           :icon="question.like? 'el-icon-star-on' : 'el-icon-star-off'"
                                           @click="like(question)">
                                    推荐{{ question.likeNum }}
                                </el-button>
                                <span>{{ question.date }}</span>
                            </div>
                        </div>
                    </div>
                </el-main>
                <el-main class="question-list" v-else>
                    <div class="question-body" v-for="question in answers">
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
                                <el-button class="recommend" type="text"
                                           :icon="question.like? 'el-icon-star-on' : 'el-icon-star-off'"
                                           @click="like(question)">
                                    推荐{{ question.likeNum }}
                                </el-button>
                                <span>{{ question.date }}</span>
                            </div>
                        </div>
                    </div>
                </el-main>
            </div>
        </el-container>
        <el-aside width="30vw">
            <el-main class="user-info">
                <div class="name-avatar">
                    {{ this.$store.state.username }}
                    <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png"
                               :size="80"></el-avatar>
                </div>
                {{ email }}
            </el-main>
            <el-main class="info">
                <span>AIApe</span>
                <span>京ICP备 2021007509号-1</span>
                <span>
                联系我们 @2021软件工程DQSJ
            </span>
            </el-main>
        </el-aside>
    </el-container>
</template>

<script>
import store from "../../vuex/store";

export default {
    data() {
        return {
            select: 'question',
            questions: [],
            answers: [],
            question: '',
            email: '',
            uid: 0
        }
    },
    methods: {
        handleSelect(selector) {
            this.$refs['scroll-body'].scrollTop = 0;
            this.select = selector;
        }
    },
    created() {
        let _this = this;
        this.$axios.get(this.BASE_URL + '/api/user/internal_info', {
            headers: {
                Authorization: 'Bearer ' + _this.$store.state.token,
                type: 'application/json;charset=utf-8'
            }
        })
        .then(function (response) {
            _this.email = response.data.email;
            _this.uid = response.data.uid;
        })
        this.$axios.get(this.BASE_URL + '/api/user/questions', {
            headers: {
                Authorization: 'Bearer ' + _this.$store.state.token,
                type: 'application/json;charset=utf-8'
            }
        })
        .then(async function (response) {
            let questionList = response.data.questions;
            for (let qid of questionList) {
                await _this.$axios.get(_this.BASE_URL + '/api/questions/question?qid=' + qid, {
                    headers: {
                        Authorization: 'Bearer ' + _this.$store.state.token,
                        type: 'application/json;charset=utf-8'
                    }
                })
                    .then(async function (response) {
                        let question = {
                            id: qid,
                            title: response.data.question.title,
                            content: response.data.question.remarks,
                            tags: response.data.question.tags,
                            date: response.data.question.createTime,
                            likeNum: response.data.question.likeNum,
                            like: response.data.question.like
                        };
                        let uid = response.data.question.creator;
                        await _this.$axios.get(_this.BASE_URL + '/api/user/public_info?uid=' + uid)
                            .then(function (response) {
                                question.creator = response.data.name;
                            })
                        _this.questions.push(question);
                    });
            }
        })
        this.$axios.get(this.BASE_URL + '/api/user/answers', {
            headers: {
                Authorization: 'Bearer ' + _this.$store.state.token,
                type: 'application/json;charset=utf-8'
            }
        })
            .then(async function (response) {
            })
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
    margin: 10px 30px;
}

i {
    color: #409eff;
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
}

.tags {
    flex-direction: row;
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

.recommend {
    height: 20px;
    font-size: 10px;
    line-height: 20px;
    padding: 3px 3px;
    margin-right: 20px;
    align-items: center;
}

.detail {
    text-overflow: ellipsis;
}

.el-aside {
    flex-direction: column;
    height: 95vh;
    overflow: visible;
    justify-content: space-between;
}

.user-info {
    box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
    background-color: white;
    border-radius: 2px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    flex-grow: 0;
    padding: 30px;
    margin-left: 10px;
}

.search-question {
    flex: 0 0 auto;
    box-shadow: 0 0 0 0;
    border-radius: 4px;
    margin-top: 0;
    margin-left: 10px;
    margin-right: 10px;
}

.info {
    box-shadow: 0 0 0 0;
    background-color: rgba(0, 0, 0, 0);
    flex-direction: column;
    align-items: center;
    flex-grow: 0;
    margin-top: 10px;
}

.name-avatar {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    font-size: 25px;
    align-self: stretch;
    margin-bottom: 20px;
}
</style>
