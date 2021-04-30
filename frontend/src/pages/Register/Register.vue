<template>
    <div class="whole">
<!--        <el-alert-->
<!--            title="注册成功!即将跳转至登录页面..."-->
<!--            type="success"-->
<!--            show-icon-->
<!--            center-->
<!--            :closable="false"-->
<!--            v-show="success">-->
<!--        </el-alert>-->
        <el-form ref="form" :model="form" label-width="70px">
            <el-form-item label="昵称">
                <el-input v-model="form.name" prefix-icon="el-icon-user" placeholder="请输入昵称"></el-input>
            </el-form-item>
            <el-form-item label="邮箱">
                <el-input v-model="form.email" prefix-icon="el-icon-message" placeholder="请输入邮箱"></el-input>
            </el-form-item>
            <el-form-item label="密码">
                <el-input v-model="form.password" prefix-icon="el-icon-lock" placeholder="请输入密码"
                          show-password></el-input>
            </el-form-item>
            <el-form-item label="确认密码">
                <el-input v-model="form.rePassword" prefix-icon="el-icon-unlock" placeholder="请确认密码"
                          show-password></el-input>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="onSubmit">注册</el-button>
            </el-form-item>
        </el-form>
    </div>
</template>

<script>
import ElementUI from 'element-ui'

export default {
    data() {
        return {
            // success: false,
            form: {
                name: '',
                email: '',
                password: '',
                rePassword: ''
            }
        }
    },
    methods: {
        onSubmit() {
            this.$message({
                message: '注册成功!即将跳转至登录页面...',
                type: 'success'
            });
            setTimeout(() => {
                this.$router.replace('/login');
            }, 2000);
        }
    }
}
</script>

<style scoped>

.whole {
    height: 100%;
    width: 100%;
    padding: 0;
    margin: 0;
}

.el-form {
    position: absolute;
    left: 32.5%;
    top: 30%;
}

.el-button {
    position: absolute;
    left: 32.5%;
}

.el-alert {
    position: absolute;
    top: 0;
    left: 27.5%;
    width: 50%;
    height: 40px;
}
</style>
