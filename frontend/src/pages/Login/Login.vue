<template>
  <el-form ref="form" :model="form" label-width="40px">
    <el-form-item label="邮箱">
      <el-input v-model="form.email" prefix-icon="el-icon-message" placeholder="请输入邮箱"></el-input>
    </el-form-item>
    <el-form-item label="密码" prop="password">
      <el-input v-model="form.password" prefix-icon="el-icon-lock" placeholder="请输入密码" show-password></el-input>
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
import axios from "axios";

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
      console.log('submit!');
      this.$router.replace('/questionList');



      // axios.get('https://aiape.snowphoenix.design/api/test/coffee',
      //   {
      //     Key: "key",
      //     Value: "value",
      //     Description: "description"
      //   })
      //   .then(function (response) {
      //     console.log(response);
      //   })
      //   .catch(function (error) {
      //     console.log(error);
      //   });

      // axios.post('https://aiape.snowphoenix.design/api/test/echojson', {
      //   firstName: 'lalala',
      //   lastName: 'Flintstone'
      // }, {
      //   // headers: {
      //   //   'Content-Type': 'application/json'
      //   // }
      // })
      //   .then(function (response) {
      //     alert('no error!')
      //     console.log(response);
      //   })
      //   .catch(function (error) {
      //     console.log(error);
      //   });

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
  left: 37.5%;
}
</style>
