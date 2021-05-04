<template>
    <div>
        <div class="log" ref="words">
            <!-- 根据vue对象中的数组，遍历出对应的标签。 -->
            <div v-for="msg in this.$store.state.logs" class="content" :class="msg.id === 1? 'user':'bot'">
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

        }
    },
    computed: {
        username() {
            return this.$store.state.username;
        },
        logs() {
            return this.$store.state.logs;
        }
    },
    methods: {
        send() {
            if (this.$store.state.username === '') {
                this.$store.commit('addAImessage', '你好,请先登录！看右边→');
                return;
            }
            if (this.$data.message === '') {
                this.$message({
                    message: '消息不能为空!',
                    type: 'warning'
                })
                return;
            }
            this.$store.commit('addUserMessage', this.$data.message);

            this.$axios.post(this.BASE_URL + '/api/ot/message', {
                message: this.$data.message
            })
                .then(function (response) {
                })
                .catch(function (error) {
                })
            this.$data.message = '';
        },
        getTagList() {
            let _this = this;
            _this.$axios.get(_this.BASE_URL + '/api/questions/taglist')
                .then(function (response) {
                    let tagList = response.data;
                    tagList = {
                        '循环': 1,
                        '语法': 2,
                        '环境': 3
                    }
                    _this.$store.commit('setTagList', tagList);
                })
        }
    },
    watch: {
        username: function (username) {
            this.$store.commit('addAImessage', '你好,' + username + '！');
        },
        logs: function () {
            this.$nextTick(() => {
                this.$refs['words'].scrollTop = this.$refs['words'].scrollHeight;
            })
        }
    },
    mounted() {
        this.getTagList();
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
