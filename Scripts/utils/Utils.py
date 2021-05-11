# -*- coding: utf8 -*-

import enum
import time
import os

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


