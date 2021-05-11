<template>
    <div>
        <div class="log" ref="words">

            <!-- 根据vue对象中的数组，遍历出对应的标签。 -->
            <div v-for="msg in this.$store.state.logs" class="content" :class="msg.id === 1? 'user':'bot'">
                <span v-html="msg.content">
                    {{ msg.content }}
                </span>
                <br/>
                <el-button class="prompt" v-for="prompt in msg.prompts"
                           :key="prompt" v-show="msg.promptValid"
                           @click="choosePrompt(prompt, msg)">
                    {{ prompt }}
                </el-button>
            </div>

        </div>
        <div class="send">
            <el-input type="textarea" resize="none" :autosize="{ minRows: 7.5, maxRows: 7.5}"
                      v-model="message"></el-input>
            <el-button class="send-button" type="primary" v-on:click="send">发送</el-button>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            message: ''
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
            let message = this.message;
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
                    let i = 0;
                    for (let message of response.data.messages) {
                        message = _this.transform(message);
                        let payload = {};
                        payload['content'] = message;
                        if (i === response.data.messages.length - 1) {
                            payload['prompts'] = response.data.prompt;
                            payload['promptValid'] = response.data.prompt.length > 0;
                        }
                        else {
                            payload['prompts'] = [];
                            payload['promptValid'] = false;
                        }
                        i++;
                        _this.$store.commit('addAImessage', payload);
                    }
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
        },
        choosePrompt(prompt, msg) {
            msg.promptValid = false;
            let _this = this;
            this.$axios.post(this.BASE_URL + '/api/bot/message', {
                message: prompt
            }, {
                headers: {
                    Authorization : 'Bearer ' + _this.$store.state.token,
                    type : 'application/json;charset=utf-8'
                }
            })
                .then(function (response) {
                    let i = 0;
                    for (let message of response.data.messages) {
                        message = _this.transform(message);
                        let payload = {};
                        payload['content'] = message;
                        if (i === response.data.messages.length - 1) {
                            payload['prompts'] = response.data.prompt;
                            payload['promptValid'] = response.data.prompt.length > 0;
                        }
                        else {
                            payload['prompts'] = [];
                            payload['promptValid'] = false;
                        }
                        i++;
                        _this.$store.commit('addAImessage', payload);
                    }
                })
                .catch(function (error) {
                })
        }
    },
    watch: {
        username: function (username) {
            let _this = this;
            this.waitTag = true;
            this.$store.commit('addAImessage', {id: 2, content: '你好,' + username + '！', prompts: [], promptValid: false});
            _this.$axios.post(_this.BASE_URL + '/api/bot/start', {}, {
                headers: {
                    Authorization : 'Bearer ' + _this.$store.state.token,
                    type : 'application/json;charset=utf-8'
                }
            })
                .then(function (response) {
                    let i = 0;
                    for (let message of response.data.messages) {
                        message = _this.transform(message);
                        let payload = {};
                        payload['content'] = message;
                        if (i === response.data.messages.length - 1) {
                            payload['prompts'] = response.data.prompt;
                            payload['promptValid'] = response.data.prompt.length > 0;
                        }
                        else {
                            payload['prompts'] = [];
                            payload['promptValid'] = false;
                        }
                        i++;
                        _this.$store.commit('addAImessage', payload);
                    }
                })
        },
        logs: function () {
            this.$nextTick(() => {
                this.$refs['words'].scrollTop = this.$refs['words'].scrollHeight;
            })
        },
        prompt: function () {
            // let message = '比如:<br/>';
            // for (let p of this.$store.state.prompt) {
            //     message += p + '<br/>';
            // }
            // this.$store.commit('addAImessage', message);
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

.send-button {
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

.prompt {
    margin-top: 10px;
}
</style>
