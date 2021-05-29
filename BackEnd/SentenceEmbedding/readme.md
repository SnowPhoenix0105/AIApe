# 序

## 目录结构

```
/
|---Demo/
|---Models/
|---.gitignore
|---const.py
|---EmbeddingApp.py
|---EmbeddingModel.py
|---readme.md
```

## 依赖包

* pytorch
* transformers
* sentence-transformers
* flask

# 句子特征获取微服务

## 获取句子特征 /api/embeddings

* 类型：POST
* 请求体：
  ```JSON
  {
      "name" : "root",
      "password" : "root@aiape.icu",
      "sentences" : ["sentence1", "sentence2"]
  }
  ```
* 请求体各部分含义：
  * `name`：管理员账户
  * `password`：账户密码
  * `sentences`：要进行特征提取的多个句子。
* 响应体：
  ```JSON
  {
      "status" : "success",
      "message" : "embeddings successfully get",
      "embeddings" : [[0.321317, 1.302184], [0.489172, 4.371628]]
  }
  ```
* 响应体各参数含义：
  * `status`：表示处理状态，`success`表示成功，其余表示失败。
  * `message`：对`status`的补充。
  * `embeddings`：对每个句子生成的特征。
* `status`取值及其含义：
  |取值|状态码|含义| 
  |:-|:-|:-|
  |`success`|`200 OK`|成功|
  |`fail`|`200 OK`|缺少语句|
  |`fail`|`401 Unauthorized`|权限验证失败|