# -*- coding: utf8 -*-

import json
from utils.Utils import LogLevel
from utils.Utils import post
from utils.Utils import set_jwt
from utils.Utils import log
from utils.Utils import protocol
from utils.Utils import base_url

root_email = "root@aiape.icu"
root_password = "aiaperoot"

tag_table = {
    "环境配置" : "环境配置相关，包括IDE安装、编译器安装。",
    "工具使用" : "开发工具使用相关，包括IDE使用、编译器使用，包括各种报错。",
    "标准库" : "标准库相关，包括标准库函数使用、理解。",
    "语句" : "语句相关，包括循环、分支等语句。",
    "关键字" : "关键字相关，包括if、while、for等。",
    "代码" : "有源代码的问题。",
    "Linux" : "由Linus Benedict Torvalds开发，并由开源社区维护的类Unix操作系统内核Linux。一般代指使用Linux内核的发行版，如Debian、Ubuntu、RHEL、Centos等。",
    "Windows" : "由微软公司（Microsoft）开发的操作系统，是目前最主流的桌面PC操作系统。",
    "macOS" : "由苹果（Apple）开发的操作系统，常用于苹果的Mac系列电脑。",
    "Dev C++" : "Windows 环境下的一个轻量级 C/C++ 集成开发环境（IDE），是一款自由软件，遵守GPL许可协议分发源代码。目前已停止维护，但仍有社区维护的版本。",
    "Visual C++" : "由微软公司（Microsoft）的免费C++集成开发环境（IDE），可提供编辑C语言。目前微软已停止维护并关闭下载渠道。",
    "VS Code" : "由微软公司（Microsoft）同社区开发的Code-OOS的构建，是一款轻量级集成开发环境（IDE），通过插件来支持多种语言的开发。",
    "Visual Studio" : "由微软公司（Microsoft）开发的重量级集成开发环境（IDE）。",
    "gcc" : "GNU Compiler Collection，GNU编译器套件，是遵循GPL协议的自由软件。支持多种语言与多种目标平台。也是GCC编译C语言所使用的的指令名。",
    "clang" : "是LLVM的C家族语言前端，是遵循BSD协议的自由软件。一般可认为是一个C语言、C++、Objective-C语言的轻量级编译器。",
    "msvc" : "由微软公司（Microsoft）开发的C++编译器，是微软旗下IDE（Visual Studio、Visual C++等）的默认C++编译器。",
}

def login() -> bool:
    res = post("/api/user/login", { "email" : root_email, "password": root_password })
    if res["status"] == "success":
        log("login success")
        set_jwt(res["token"])
        return True
    else:
        log(res["message"])
        return False

def add_tag(name: str, desc: str) -> int:
    obj = { "name": name, "desc": desc }
    log("添加标签：", json.dumps(obj))
    res = post("/api/questions/add_tag", obj)
    if res["status"] != "success":
        log("添加标签：", json.dumps(obj), "失败", "ret=", res, level=LogLevel.WAR)
        return -1
    else:
        tid = res["tid"]
        log("添加标签：", json.dumps(obj), "成功", "tid=", tid)
        return tid

def add_question(title: str, remarks: str, tags: list) -> int:
    obj = { "title": title, "remarks": remarks, "tags": [tag_table[t] for t in tags] }
    log("添加问题：", json.dumps(obj))
    res = post("/api/questions/add_tag", obj)
    if res["status"] != "success":
        log("添加问题：", json.dumps(obj), "失败", "ret=", res, level=LogLevel.WAR)
        return -1
    else:
        qid = res["qid"]
        log("添加问题：", json.dumps(obj), "成功", "qid=", qid)
        return qid

def add_answer(qid: int, content: str) -> int:
    obj = { "qid": qid, "content": content }
    log("添加回答：", json.dumps(obj))
    res = post("/api/questions/add_tag", obj)
    if res["status"] != "success":
        log("添加回答：", json.dumps(obj), "失败", "ret=", res, level=LogLevel.WAR)
        return -1
    else:
        aid = res["aid"]
        log("添加回答：", json.dumps(obj), "成功", "aid=", aid)
        return aid

def try_add_all_tags():
    for k, v in tag_table.items():
        add_tag(k, v)

def main() -> int:
    log("using protocol:", protocol)
    log("using base_url:", base_url)
    if not login():
        log("login fail", level=LogLevel.ERR)
        return -1
    try_add_all_tags()
    return 0

if __name__ == '__main__':
    ret = main()
    exit(ret)
