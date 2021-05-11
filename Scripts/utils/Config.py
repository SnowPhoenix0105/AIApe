# -*- coding: utf8 -*-

import sys
import os
from .Utils import pcat


class Path:
    Script = sys.path[0]
    Top = pcat(Script, "..")
    BackEnd = pcat(Top, "BackEnd")
    AIBot = pcat(BackEnd, "AIBot")
    AIBotTest = pcat(BackEnd, "AIBotTest")
    AIBotTest_TestResults = pcat(AIBotTest, "TestResults")

if __name__ == "__main__":
    table = {
        "Script": Path.Script,
        "Top": Path.Top,
        "BackEnd": Path.BackEnd,
        "AIBot": Path.AIBot,
        "AIBotTest": Path.AIBotTest,
        "AIBotTest_TestResults": Path.AIBotTest_TestResults,
    }
    max_length = max(map(len, table.keys()))
    for k, v in table.items():
        print(k.rjust(max_length), ":", v)
    