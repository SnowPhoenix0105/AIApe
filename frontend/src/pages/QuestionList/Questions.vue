<template>
    <div class="outside">
        <el-table
            :data="questions"
            style="width: 100%">
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
                        <el-link :href="scope.row.link" slot="reference">{{ scope.row.title }}</el-link>
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
            _this.$axios.post('https://aiape.snowphoenix.design/api/test/questions/questionlist', {
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
                                    'title': response.data.message,
                                    'link': 'http://www.baidu.com'
                                });
                            });
                    }
                })
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
    margin-left: 20px;
}

</style>
