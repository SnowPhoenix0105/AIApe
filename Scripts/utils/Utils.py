# -*- coding: utf8 -*-

import enum
import time
import os
import json
import urllib.request

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



class NoAuthorization(Exception):
    pass

class NotFound(Exception):
    pass

jwt = ""
protocol = "http"
base_urls = [
        "test.snowphoenix.design", 
        "localhost:5000", 
        "aiape.snowphoenix.design"]
base_url = base_urls[0]

def set_jwt(token: str):
    global jwt
    jwt = token

def post(target: str, body, headers: dict=None):
    body = json.dumps(body)
    if headers is None:
        headers = {}
    headers["Content-Type"] = "application/json"
    if len(jwt) != 0:
        headers["Authorization"] = "Bearer " + jwt

    origin_host =  protocol + "://" + base_url
    url = origin_host + target

    req = urllib.request.Request(url, data=body.encode("utf8"), headers=headers, origin_req_host=origin_host, method="POST")

    try:
        with urllib.request.urlopen(req) as f:
            raw_json = f.read().decode("utf8")
    except urllib.error.HTTPError as e:
        if e.code == 401:
            raise NoAuthorization()
        elif e.code == 401:
            raise NotFound()

    # print("[RAW]:\t", raw_json)
    return json.loads(raw_json)
