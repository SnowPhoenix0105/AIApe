# -*- coding: utf8 -*-

import enum
import time
import os
import sys

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




class StopException(Exception):
    def __init__(self, *args, **kargs):
        super().__init__(*args, **kargs)

def exec(command: str, ensure_success: bool=True) -> int:
    log("executing command: {}".format(command))
    sys.stdout.flush()
    ret = os.system(command)
    log("executed command with exit-code={}".format(ret))
    if ensure_success and ret != 0:
        raise StopException("exit because command exited with error: {}".format(command))
    return ret