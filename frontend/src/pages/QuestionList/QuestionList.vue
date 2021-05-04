<template>
    <div class="outside">
        <div>
            <!--            <el-button type="primary" v-on:click="goToPersonalCenter">个人中心</el-button>-->
            <el-dropdown @command="handleCommand">
                <span class="el-dropdown-link">
                    菜单<i class="el-icon-arrow-down el-icon--right"></i>
                </span>
                <template #dropdown>
                    <el-dropdown-menu>
                        <el-dropdown-item command="personalCenter">个人中心</el-dropdown-item>
                        <el-dropdown-item command="administration">管理员后台</el-dropdown-item>
                    </el-dropdown-menu>
                </template>
            </el-dropdown>

        </div>
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
                        <p>编号: {{ scope.row.id }}</p>
                        <p>问题: {{ scope.row.title }}</p>
                        <el-link @click="goToDetail(scope.row.id)" slot="reference">{{ scope.row.title }}</el-link>
                    </el-popover>
                </template>
            </el-table-column>
        </el-table>
    </div>
</template>

<script>
export default {
    data() {
        return {
            questions: []
        }
    },
    mounted() {
        this.getQuestions();
    },
    methods: {
        getQuestions() {
            let _this = this;
            _this.$axios.post(_this.BASE_URL + '/api/test/questions/questionlist', {
                number: 70
            })
                .then(function (response) {
                    let questionIdList = response.data;
                    questionIdList.sort();
                    for (let qid of questionIdList) {
                        _this.$axios.get('https://aiape.snowphoenix.design/api/test/questions/question?qid=' + qid)
                            .then(function (response) {
                                _this.$data.questions.push({
                                    'id': qid,
                                    'title': response.data.question.title
                                });
                            });
                    }
                })
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

</style>
