# -*- coding: utf8 -*-

import os
import sys
import enum
import time
import xml.etree.ElementTree as ET

class LogLevel(enum.Enum):
    DBG = 0
    INF = 1
    WAR = 2
    ERR = 3

def get_time_stampe():
    return time.strftime("%Y-%m-%d %H:%M:%S", time.localtime(time.time()))

def log(*msg, level = LogLevel.INF):
    print("{}: [{}] {}".format(get_time_stampe(), level.name, " ".join(str(o) for o in msg)))

pcat = lambda p, c : os.path.abspath(os.path.join(p, c))

class Path:
    Script = sys.path[0]
    Top = pcat(Script, "..")
    BackEnd = pcat(Top, "BackEnd")
    AIBot = pcat(BackEnd, "AIBot")
    AIBotTest = pcat(BackEnd, "AIBotTest")
    AIBotTest_TestResults = pcat(AIBotTest, "TestResults")

class StopException(Exception):
    def __init__(self, *args, **kargs):
        super.__init__(self, *args, **kargs)

def exec(command: str, ensure_success: bool=True) -> int:
    log("executing command: {}".format(command))
    ret = os.system(command)
    log("executed command with exit-code={}".format(ret))
    if ensure_success and ret != 0:
        raise StopException("exit because command exited with error: {}".format(command))
    return ret

def restore():
    log("恢复项目依赖")
    command = "dotnet restore {}".format(Path.BackEnd)
    exec(command)

def build_without_restore():
    log("构建项目")
    command = "dotnet build --no-restore {}".format(Path.BackEnd)
    exec(command)

def test_without_build()->str:
    log("运行测试")
    old_dir = set(os.listdir(Path.AIBotTest_TestResults))
    command = "dotnet test --no-build --collect:\"XPlat Code Coverage\" {}".format(Path.BackEnd)
    exec(command)
    new_dir = set(os.listdir(Path.AIBotTest_TestResults))
    news = new_dir - old_dir
    assert len(news) == 1
    guid = next(iter(news))
    log("test result guid={}".format(guid))
    return guid

def install_tool_ReportGenerator():
    log("安装覆盖报告生成工具")
    command = "dotnet tool install -g dotnet-reportgenerator-globaltool"
    exec(command, ensure_success=False)

def generate_report(guid: str):
    log("生成覆盖报表")
    result_path = pcat(Path.AIBotTest_TestResults, guid)
    xml_path = pcat(result_path, "coverage.cobertura.xml")
    report_path = pcat(result_path, "coverage_report")
    command = "reportgenerator " +\
            "\"-reports:{}\" ".format(xml_path) +\
            "\"-targetdir:{}\" ".format(report_path) +\
            "-reporttypes:Html"
    exec(command)

def print_cli_message(guid: str):
    result_path = pcat(Path.AIBotTest_TestResults, guid)
    xml_path = pcat(result_path, "coverage.cobertura.xml")
    etree = ET.parse(xml_path)
    root = etree.getroot()
    attr = root.attrib
    line_rate = float(attr["line-rate"])
    branch_rate = float(attr["branch-rate"])
    lines_covered = int(attr["lines-covered"])
    lines_valid = int(attr["lines-valid"])
    branches_valid = int(attr["branches-valid"])
    branches_covered = int(attr["branches-covered"])
    log("test-case-GUID:\t{}".format(guid))
    log("line-coverage:\t{:.2f} % ({:d}/{:d})".format(line_rate * 100, lines_covered, lines_valid))
    log("branch-coverage:\t{:.2f} % ({:d}/{:d})".format(branch_rate * 100, branches_covered, branches_valid))

    
def main():
    try:
        restore()
        build_without_restore()
        guid = test_without_build()
        try:
            install_tool_ReportGenerator()
            generate_report(guid)
        except StopException as e:
            log(str(e), LogLevel.WAR)
        finally:
            print_cli_message(guid)
    except StopException as e:
        log(str(e), LogLevel.WAR)

if __name__ == '__main__':
    main()
