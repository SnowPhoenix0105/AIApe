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
                    <el-button type="primary" @click="">查询</el-button>
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
            <h2>问题管理</h2>
            <el-form ref="form" :model="questionForm" label-width="100px">
                <el-form-item label="问题id">
                    <el-input v-model="questionForm.uid"></el-input>
                </el-form-item>
                <el-form-item label="问题题目">
                    <el-input v-model="questionForm.question"></el-input>
                </el-form-item>
                <el-form-item label="问题内容">
                    <el-input v-model="questionForm.remarks"></el-input>
                </el-form-item>
                <el-form-item label="问题标签">
                    <el-input v-model="questionForm.tags"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="">删除问题</el-button>
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
            <h2>回答管理</h2>
            <el-form ref="form" :model="answerForm" label-width="100px">
                <el-form-item label="回答id">
                    <el-input v-model="answerForm.aid"></el-input>
                </el-form-item>
                <el-form-item label="回答内容">
                    <el-input v-model="answerForm.content"></el-input>
                </el-form-item>
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
            },
            questionForm: {
                qid: '',
                question: '',
                remarks: '',
                tags: [1,2,3],
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
