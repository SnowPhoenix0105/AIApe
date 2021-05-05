<template>
<!--    <<<<<<< HEAD-->
<!--    <div class="outside">-->
<!--        <div>-->
<!--            &lt;!&ndash;            <el-button type="primary" v-on:click="goToPersonalCenter">个人中心</el-button>&ndash;&gt;-->
<!--            <el-dropdown @command="handleCommand">-->
<!--                <span class="el-dropdown-link">-->
<!--                    菜单<i class="el-icon-arrow-down el-icon&#45;&#45;right"></i>-->
<!--                </span>-->
<!--                <template #dropdown>-->
<!--                    <el-dropdown-menu>-->
<!--                        <el-dropdown-item command="personalCenter">个人中心</el-dropdown-item>-->
<!--                        <el-dropdown-item command="administration">管理员后台</el-dropdown-item>-->
<!--                    </el-dropdown-menu>-->
<!--                </template>-->
<!--            </el-dropdown>-->

<!--        </div>-->
<!--        <el-table-->
<!--            :data="questions"-->
<!--            style="width: 100%"-->
<!--            :header-cell-style="{textAlign: 'center'}"-->
<!--            :cell-style="{ textAlign: 'center' }">-->
<!--            <el-table-column-->
<!--                prop="id"-->
<!--                label="编号"-->
<!--                width="180">-->
<!--            </el-table-column>-->
<!--            <el-table-column-->
<!--                label="问题">-->
<!--                <template slot-scope="scope">-->
<!--                    <el-popover trigger="hover" placement="top">-->
<!--                        <p>编号: {{ scope.row.id }}</p>-->
<!--                        <p>问题: {{ scope.row.title }}</p>-->
<!--                        <el-link @click="goToDetail(scope.row.id)" slot="reference">{{ scope.row.title }}</el-link>-->
<!--                    </el-popover>-->
<!--                </template>-->
<!--            </el-table-column>-->
<!--        </el-table>-->
<!--    </div>-->
<!--    =======-->
    <el-container>
        <el-header>
            <el-link :underline="false" @click="goToPersonalCenter">{{ this.$store.state.username }}</el-link>
            <el-link :underline="false" disabled style="cursor: default">|</el-link>
            <el-link :underline="false">注销</el-link>
            <el-link :underline="false" @click="gotoAdministration">管理员后台</el-link>
        </el-header>
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
                <el-table-column
                    prop="id"
                    label="编号"
                    width="180">
                </el-table-column>
                <el-table-column
                    label="问题">
                    <template slot-scope="scope">
                        <el-popover trigger="hover" placement="top">
                            <p>{{ scope.row.content }}</p>
                            <el-link @click="goToDetail(scope.row.id)" slot="reference">{{ scope.row.title }}</el-link>
                        </el-popover>
                    </template>
                </el-table-column>
            </el-table>
        </el-main>
    </el-container>
<!--    >>>>>>> 19e2b81d9fd5820fd0bdb44fb8454a18f265ea3a-->
</template>

<script>
export default {
    data() {
        return {
            questions: [],
            selectedTag: [],
            tagState: {},
            showTag: true
        }
    },
    mounted() {
        this.getQuestions();
        this.initTagState();
    },
    methods: {
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
            for (let tagname in tagList) {
                this.$data.tagState[tagList[tagname]] = false;
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

        }
    }
}
</script>

<style scoped>
.outside {
    height: 100%;
    overflow: hidden;
}

.el-table {
    -ms-flex: none !important;
    flex: none !important;
    overflow: scroll;
    height: 100%;
    margin-left: 1px;
}


.el-dropdown {
    margin: 20px;
    float: right;
}

.el-header {
    display: flex;
    justify-content: flex-end;
}

.el-link {
    margin: 5px;
}

.el-main {
    display: flex;
}

.el-tag {
    cursor: pointer;
    margin: 5px;
}

.tag-selector {
    padding-top: 0;
}

.table {
    padding: 0;
    overflow-x: hidden;

}

</style>
