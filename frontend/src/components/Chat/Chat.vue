<template>
    <div>
        <div class="log" ref="words">

            <!-- 根据vue对象中的数组，遍历出对应的标签。 -->
            <div v-for="msg in this.$store.state.logs" class="content" :class="msg.id === 1? 'user':'bot'">
                <span v-html="msg.content">
                    {{ msg.content }}
                </span>
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
        },
        prompt() {
            return this.$store.state.prompt;
        }
    },
    methods: {
        send() {
            let _this = this;
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

            this.$axios.post(this.BASE_URL + '/api/bot/message', {
                message: this.$data.message
            }, {
                headers: {
                    Authorization : 'Bearer ' + _this.$store.state.token,
                    type : 'application/json;charset=utf-8'
                }
            })
                .then(function (response) {
                    for (let message of response.data.messages) {
                        message = _this.transform(message);
                        _this.$store.commit('addAImessage', message);
                    }
                    _this.$store.commit('setPrompt', response.data.prompt);
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
        },
        transform(msg) {
            msg = msg.replaceAll('\n', '<br/>');
            let left, right;
            left = msg.indexOf('[');
            right = msg.indexOf(']');
            while (left !== -1) {
                let space = msg.substring(left, right).indexOf(' ');
                let url;
                if (space !== -1) {
                    url = msg.substring(left + space + 1, right);
                }
                else {
                    url = msg.substring(left + 1, right);
                }
                msg = msg.substring(0, left) + '<a target="_blank" style="color: white" href="' + url + '">' + url + '</a>'+ msg.substring(right + 1);
                left = msg.indexOf('[');
                right = msg.indexOf(']');
            }
            return msg;
        }
    },
    watch: {
        username: function (username) {
            let _this = this;
            this.$store.commit('addAImessage', '你好,' + username + '！');
            _this.$axios.post(_this.BASE_URL + '/api/bot/start', {}, {
                headers: {
                    Authorization : 'Bearer ' + _this.$store.state.token,
                    type : 'application/json;charset=utf-8'
                }
            })
                .then(function (response) {
                    for (let message of response.data.messages) {
                        message = _this.transform(message);
                        _this.$store.commit('addAImessage', message);
                    }
                    _this.$store.commit('setPrompt', response.data.prompt);
                })
        },
        logs: function () {
            this.$nextTick(() => {
                this.$refs['words'].scrollTop = this.$refs['words'].scrollHeight;
            })
        },
        prompt: function () {
            if (this.$store.state.prompt.length === 0) {
                return;
            }
            let message = '比如:<br/>';
            for (let p of this.$store.state.prompt) {
                message += p + '<br/>';
            }
            this.$store.commit('addAImessage', message);
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
    height: 72%;
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
    background: #409EFF;
    border-radius: 10px;
    color: #fff;
    padding: 5px 10px;
    left: 10px;
}

.user {
    margin: 10px;
    text-align: right;
    width: auto;
    height: auto;
}

.bot {
    height: auto;
    margin: 10px;
    width: auto;
}

.user span {
    display: inline-block;
    background: #ffb449;
    border-radius: 10px;
    color: #fff;
    padding: 5px 10px;
    right: 10px;
}

a:visited {
    color: #409EFF;
}
</style>
