<template>
    <el-form ref="form" :model="form" label-width="40px">
        <el-form-item label="邮箱">
            <el-input v-model="form.email" prefix-icon="el-icon-message" placeholder="请输入邮箱"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
            <el-input v-model="form.password" prefix-icon="el-icon-lock" @keyup.native.enter="onSubmit" placeholder="请输入密码" show-password></el-input>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" @click="onSubmit">登录</el-button>
        </el-form-item>
        <br/>
        <el-link type="info" @click="goToRegister()">还没有账号?点击注册</el-link>
    </el-form>
</template>

<script>
import Chat from '../../components/Chat/Chat.vue'

export default {
    data() {
        return {
            form: {
                email: '',
                password: ''
            }
        }
    },
    methods: {
        onSubmit() {
            let _this = this;
            this.$axios.post(this.BASE_URL + "/api/user/login", {
                email: _this.$data.form.email,
                password: _this.$data.form.password
            })
            .then(function (response) {
                console.log(response);
                if (response.data.status === "fail") {
                    _this.$message({
                        message: '邮箱或密码错误!',
                        type: 'error'
                    });
                }
                else {
                    _this.$message({
                        message: '登录成功!',
                        type: 'success'
                    });
                    _this.$store.commit('setToken', response.data.token);
                    _this.$store.commit('setAuth', response.data.auth);
                    _this.$store.commit('setTimeout', response.data.timeout);
                    _this.$router.replace('/questionList');
                    _this.$axios.get(_this.BASE_URL + '/api/user/full_info')
                    .then(function (response) {
                        console.log(response);
                        _this.$store.commit('setUsername', response.data.name);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
                }
            })
        },
        goToRegister() {
            this.$router.replace('/register');
        }
    }
}
</script>

<style scoped>

.el-form {
    position: absolute;
    left: 35%;
    top: 35%;
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
