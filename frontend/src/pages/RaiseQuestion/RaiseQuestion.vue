<template>
    <el-container class="shell">
        <el-header>
            AIApe
        </el-header>
        <el-container class="main-body">
            <el-aside width="100px">
                <el-steps direction="vertical" :active="step">
                    <el-step title="问题标题"></el-step>
                    <el-step title="选择标签"></el-step>
                    <el-step title="问题详情"></el-step>
                </el-steps>
            </el-aside>
            <el-main>
                <div class="edit-title" v-show="step === 1">
                    <el-input v-show="step === 1" v-model="title"></el-input>
                    <el-button v-show="step === 1" @click="submitTitle">提交</el-button>
                </div>
                <transition name="slide" enter-active-class="slideInUp">
                    <div class="title" v-show="step > 1">
                        <span>{{ title }}</span>
                    </div>
                </transition>
                <transition name="slide" enter-active-class="slideInUp">
                    <div class="select-tag" v-show="step === 2">
                        <div v-for="(ls, type) in tags">
                            <span>{{ type }}</span>
                            <span>{{ ls }}</span>
                        </div>
                        <el-button @click="submitTags">完成</el-button>
                    </div>
                </transition>
                <transition name="slide" enter-active-class="slideInUp">
                    <div class="tag" v-show="step > 2">
                        <el-tag v-for="tag in selectTags" :key="tag">{{ tag }}</el-tag>
                    </div>
                </transition>
                <div class="edit-detail" v-show="step === 3">

                </div>
            </el-main>
        </el-container>
    </el-container>
</template>

<script>
export default {
    data() {
        return {
            step: 1,
            title: '',
            detail: '',
            selectTags: ['C', 'Linux', '语法'],
            tags: {
                '语言': ['C', 'Python', 'C++', 'Java'], '环境': ['Linux', 'Windows', 'MacOS'],
                '类型': ['语法', '代码', '环境', '算法', '题目']
            }
        }
    },
    methods: {
        submitTitle() {
            this.step = 2;
        },
        submitTags() {
            this.step = 3;
        }
    }
}
</script>

<style scoped>
.shell {
    position: absolute;
    left: 5vw;
    top: 0;
    width: 95vw;
    height: 100vh;
    background-color: rgba(246, 246, 246, 1);
    flex-direction: column;
    align-items: stretch;
    padding: 50px 0;
}

.el-aside {
    margin-left: 50px;
    padding: 50px 0;
}

.main-body {
    flex-direction: row;
    align-items: stretch;
    margin-right: 150px;
}

.el-main {
    flex-direction: column;
    align-items: stretch;
    justify-content: flex-start;
}

.el-header {
    font-size: 20px;
    font-weight: bold;
    align-self: center;
}

.el-main > * {
    display: flex;
    flex-direction: column;
    align-items: stretch;
    margin: 0 100px;
}

.el-button {
    align-self: flex-end;
}
</style>
