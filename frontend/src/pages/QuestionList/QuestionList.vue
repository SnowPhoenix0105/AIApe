<template>
  <div>
<!--    <div class="header">-->
<!--      <h1>问题列表</h1>-->
<!--    </div>-->
<!--    <div class="down">-->
<!--      <ul class="log">-->
<!--        <li v-for="item of this.tableData"><span>{{ item }}</span></li>-->
<!--      </ul>-->
<!--    </div>-->
    <div class="header">
      <h1>问题列表</h1>
    </div>
    <div class="down">
      <ul class="log">
        <li v-for="item of this.tableData"><span>{{ item }}</span></li>
      </ul>
    </div>
    <el-table
      :data="tableData"
      style="width: 100%">
      <el-table-column
        prop="date"
        label="日期"
        width="180">
      </el-table-column>
      <el-table-column
        prop="name"
        label="姓名"
        width="180">
      </el-table-column>
      <el-table-column
        prop="address"
        label="地址">
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      tableData: [],
      // idx: [],
    }
  },
  mounted() {
    var __this = this;
    axios.post('https://aiape.snowphoenix.design/api/test/questions/questionlist',
      {
        number: 70
      }
    ).then(function (response) {
      console.log("qid列表如下");
      let qidLs = response.data;
      console.log(qidLs);

      for (let idx = 0; idx < qidLs.length; idx++) {
        let qid = qidLs[idx];
        axios.get('https://aiape.snowphoenix.design/api/test/questions/question?qid=' + qid)
          .then(function (response) {
            __this.tableData.push(response.data.message);
          })
      }
    })
  },
  name: "QuestionList.vue",
  methods: {
    showIdx() {
      alert(this.idx);
      for (var i = 0; i < this.idx.length; i++) {
        var ii = this.idx[i];
        concole.log(ii);
        var __this = this;
        // console.log(ii);
        axios.get('https://aiape.snowphoenix.design/api/test/questions/question?qid=' + ii)
          .then(function (response) {
            __this.tableData[__this.tableData.length] = ii + response.data.message;
            console.log(ii + response.data.message);
          })
      }
    },
    showData() {
      alert(this.tableData);
    },
    addData() {
      this.tableData.push("lalala")
    }
  }
}
</script>

<style scoped>
.log {
  position: absolute;
  height: 85%;
  overflow: scroll;
  width: 95%;
}

.down {
  width:100%;
  height:90%;
}

.header {
  width:100%;
  height:10%;
  background-color: lightgray;
}
</style>
