
import json
import urllib.request
import urllib.error
from .Utils import log
from .Utils import LogLevel

class NoAuthorization(Exception):
    pass

class NotFound(Exception):
    pass

default_jwt = ""
protocol = "http"
base_urls = [
        "test.snowphoenix.design", 
        "localhost:5000", 
        "aiape.snowphoenix.design"]
base_url = base_urls[1]

def set_default_jwt(token: str):
    global default_jwt
    default_jwt = token

def login() -> str:
    while True:
        email = input("email:\t")
        password = input("password:\t")
        rsp = post("/api/user/login", { "email" : email, "password": password})
        if rsp["status"] == "success":
            print("login success")
            token = rsp["token"]
            set_default_jwt(token)
            return token
        else:
            print(rsp["message"])

def signup():
    flag = True
    while flag:
        name = input("name:\t")
        email = input("email:\t")
        password = input("password:\t")
        rsp = post("/api/user/signup", { "name" : name, "email" : email, "password": password})
        if rsp["status"] == "success":
            flag = False
            print("signup success")
        else:
            print(rsp["message"])

def get(target: str, headers: dict=None, jwt: str=None):
    global default_jwt
    jwt = jwt if jwt is not None else default_jwt
    if headers is None:
        headers = {}
    if jwt is not None and len(jwt) != 0:
        headers["Authorization"] = "Bearer " + jwt

    origin_host =  protocol + "://" + base_url
    url = origin_host + target

    req = urllib.request.Request(url, headers=headers, origin_req_host=origin_host, method="GET")

    try:
        with urllib.request.urlopen(req) as f:
            raw_json = f.read().decode("utf8")
    except urllib.error.HTTPError as e:
        if e.code == 401:
            raise NoAuthorization()
        elif e.code == 404:
            raise NotFound()
        log(e.headers, level=LogLevel.WAR)
        log(e.fp.read(), level=LogLevel.WAR)
        raise e

    # print("[RAW]:\t", raw_json)
    return json.loads(raw_json)

def post(target: str, body, headers: dict=None, jwt: str=None):
    global default_jwt
    jwt = jwt if jwt is not None else default_jwt
    body = json.dumps(body)
    if headers is None:
        headers = {}
    headers["Content-Type"] = "application/json"
    if jwt is not None and len(jwt) != 0:
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
        elif e.code == 404:
            raise NotFound()
        log(e.headers, level=LogLevel.WAR)
        log(e.fp.read(), level=LogLevel.WAR)
        raise e

    # print("[RAW]:\t", raw_json)
    return json.loads(raw_json)