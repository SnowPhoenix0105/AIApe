<template>
    <div>
        <div class="log" ref="words">
            <!-- 根据vue对象中的数组，遍历出对应的标签。 -->
            <div v-for="msg in logs" class="content" :class="msg.id === 1? 'user':'bot'">
                <span>{{ msg.content }}</span>
            </div>
        </div>
        <div class="send">
            <el-input type="textarea" resize="none" :autosize="{ minRows: 7.5, maxRows: 7.5}"
                      v-model="message"></el-input>
            <el-button type="primary" v-on:click="send">发送</el-button>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            message: '',
            logs: [{id: 2, content: '你好，我是AIApe!请先登录！'}]
        }
    },
    computed: {
        username() {
            return this.$store.state.username;
        }
    },
    methods: {
        send() {
            if (this.$data.message === '') {
                this.$message({
                    message: '消息不能为空!',
                    type: 'warning'
                })
                return;
            }
            this.$data.logs.push({id: 1, content: this.$data.message});

            this.$axios.post(this.BASE_URL + '/api/ot/message', {
                message: this.$data.message
            })
            .then(function (response) {
                console.log(response);
            })
            .catch(function (error) {
                window.alert('error!');
                console.log(error);
            })
            this.$data.message = '';
            this.$nextTick(() => {
                this.$refs['words'].scrollTop = this.$refs['words'].scrollHeight;
            })
        },
    },
    watch: {
        username: function (username) {
            this.$data.logs.push({id: 2, content: username});
        }
    }
}
</script>

<style scoped>

div {
    height: 100%;
    width: 100%;
    padding: 0;
    margin: 0;
}

.log {
    position: absolute;
    height: 70%;
    top: 20px;
    overflow: scroll;
}

.send {
    position: absolute;
    height: 30%;
    top: 70%;
}

.el-button {
    position: absolute;
    right: 2px;
    bottom: 4px;
    padding-top: 10px;
    height: 35px;
}

.el-textarea {
    height: 100%;
    font-family: "Microsoft YaHei", serif;
    font-size: 18px;
}

.content {
    height: 40px;
    width: 100%;
}

.bot span {
    display: inline-block;
    background: #0181cc;
    border-radius: 10px;
    color: #fff;
    padding: 5px 10px;
    position: absolute;
    left: 10px;
}

.user {
    margin: 10px;
    text-align: right;
    height: 30px;
    width: 100%;
}

.user span {
    display: inline-block;
    background: #ef8201;
    border-radius: 10px;
    color: #fff;
    padding: 5px 10px;
    position: absolute;
    right: 10px;
}

</style>
