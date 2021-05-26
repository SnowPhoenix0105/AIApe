<template>
    <div>
        <h1 class="title">AIAPE-注册</h1>
        <div class="form">
            <el-form label-position="left" label-width="25%" :model="registerForm">
                <el-form-item label="呢称">
                    <el-input prefix-icon="el-icon-user" v-model="registerForm.nickName"></el-input>
                </el-form-item>
                <el-form-item label="邮箱">
                    <el-input prefix-icon="el-icon-message" v-model="registerForm.email"></el-input>
                </el-form-item>
                <el-form-item label="密码">
                    <el-input type="password" prefix-icon="el-icon-lock"
                              show-password v-model="registerForm.password"></el-input>
                </el-form-item>
                <el-form-item label="确认密码">
                    <el-input type="password" prefix-icon="el-icon-unlock" show-password
                              v-model="registerForm.rePassword"></el-input>
                </el-form-item>
            </el-form>
        </div>
        <div align="center">
            <el-button type="primary" v-on:click="register">注册</el-button>
        </div>

        <div align="center" class=gtl>
            <el-link :underline="true" target="_blank" v-on:click="goToLogin">返回登陆</el-link>
        </div>
    </div>
</template>

<script>
export default {
    name: "MobileRegister",
    data() {
        return {
            registerForm: {
                nickName: '',
                email: '',
                password: '',
                rePassword: '',
            },
            rePasswordError: false,
        }
    },
    methods: {
        register() {

        },
        goToLogin() {
            this.$store.state.mobileStatus = 'login';
        },
        checkPassword() {
            this.$data.rePasswordError = this.$data.registerForm.password !== this.$data.registerForm.rePassword;
        }
    }
}
</script>

<style scoped>

.title {
    text-align: center;
}

.form {
    width: 70%;
    margin-left: 10%;
    margin-right: 20%;
}

.gtl {
    margin-top: 10%;
}

</style>
