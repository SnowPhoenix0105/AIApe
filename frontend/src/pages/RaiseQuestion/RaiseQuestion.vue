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
                        <h1>{{ title }}</h1>
                    </div>
                </transition>
                <transition name="slide" enter-active-class="slideInUp" leave-active-class="slideOutUp">
                    <div class="select-tag" v-show="step === 2">
                        <div v-for="(ls, type) in tags" class="tag-type">
                            <span>{{ type }}</span>
                            <el-tag v-for="tag in ls" :key="tag">{{ tag }}</el-tag>
                        </div>
                        <el-button type="primary" @click="submitTags">完成</el-button>
                    </div>
                </transition>
                <transition name="slide" enter-active-class="slideInUp">
                    <div class="tag" v-show="step > 2">
                        <el-tag v-for="tag in selectTags" :key="tag">{{ tag }}</el-tag>
                    </div>
                </transition>
                <transition name="slide" enter-active-class="slideInUp">
                    <div class="edit-detail" v-show="step === 3">
                        <mavon-editor :toolbars="toolbars" v-model="detail" ref=md
                                      :subfield="prop.subfield" :defaultOpen="prop.defaultOpen"
                                      :toolbarsFlag="prop.toolbarsFlag" :editable="prop.editable"
                                      :scrollStyle="prop.scrollStyle" :boxShadow="prop.boxShadow"
                                      style="min-height: 50vh; max-height: 0"
                                      placeholder="详细描述你的问题...">

                        </mavon-editor>
                        <el-button type="primary">提交</el-button>
                    </div>
                </transition>
            </el-main>
        </el-container>
    </el-container>
</template>

<script>
export default {
    data() {
        return {
            step: 1,
            title: '这是问题标题',
            detail: '',
            selectTags: ['C', 'Linux', '语法'],
            tags: {
                '语言': ['C', 'Python', 'C++', 'Java'], '环境': ['Linux', 'Windows', 'MacOS'],
                '类型': ['语法', '代码', '环境', '算法', '题目']
            },

            toolbars: {
                bold: true, // 粗体
                italic: true, // 斜体
                header: true, // 标题
                underline: true, // 下划线
                strikethrough: false, // 中划线
                mark: false, // 标记
                superscript: true, // 上角标
                subscript: true, // 下角标
                quote: false, // 引用
                ol: true, // 有序列表
                ul: true, // 无序列表
                link: true, // 链接
                imagelink: false, // 图片链接
                code: false, // code
                table: true, // 表格
                fullscreen: true, // 全屏编辑
                readmodel: false, // 沉浸式阅读
                htmlcode: false, // 展示html源码
                help: true, // 帮助
                /* 1.3.5 */
                undo: true, // 上一步
                redo: true, // 下一步
                trash: true, // 清空
                save: false, // 保存（触发events中的save事件）
                /* 1.4.2 */
                navigation: false, // 导航目录
                /* 2.1.8 */
                alignleft: true, // 左对齐
                aligncenter: true, // 居中
                alignright: true, // 右对齐
                /* 2.2.1 */
                subfield: true, // 单双栏模式
                preview: true // 预览
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
    },
    computed: {
        prop() {
            let data = {
                subfield: false,// 单双栏模式
                defaultOpen: 'edit',//edit： 默认展示编辑区域 ， preview： 默认展示预览区域
                editable: true,
                toolbarsFlag: true,
                scrollStyle: false,
                boxShadow: true//边框
            };
            return data;
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
    padding: 20px 0;
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
    margin-top: 10px;
}

h1 {
    margin: 0;
    font-size: 20px;
}
.edit-title {
    margin-top: 40px;
}

.title {
    margin-top: 40px;
}

.select-tag {
    margin-top: 40px;
}

.tag-type {
    flex-direction: row;
    margin-bottom: 20px;
}

.tag {
    flex-direction: row;
    margin-top: 40px;
}

.tag .el-tag {
    cursor: default;
}

.el-tag {
    margin-right: 10px;
}

.edit-detail {
    margin-top: 40px;
}

</style>
