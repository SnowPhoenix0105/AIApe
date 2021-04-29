import http.client
import json

jwt = ""

class NoAuthorization(Exception):
    pass

def post(target: str, body, headers: dict=None):
    body = json.dumps(body)
    if headers is None:
        headers = {}
    headers["Content-Type"] = "application/json"
    conn = http.client.HTTPConnection("localhost:5000")
    conn.request("POST", target, body=body, headers=headers)
    rsp = conn.getresponse()
    if rsp.status == 401:
        raise NoAuthorization()
    raw_json = rsp.read()
    # print("[RAW]:\t", raw_json)
    return json.loads(raw_json)

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

def login():
    global jwt
    flag = True
    while flag:
        email = input("email:\t")
        password = input("password:\t")
        rsp = post("/api/user/login", { "email" : email, "password": password})
        if rsp["status"] == "success":
            flag = False
            print("login success")
            jwt = rsp["token"]
        else:
            print(rsp["message"])

def print_bot_message(rsp: http.client.HTTPResponse):
    messages = rsp["message"].split("\n")
    print(">>>\t", "\n\t".join(messages))
    prompt = rsp["prompt"]
    if len(prompt) != 0:
        print(prompt)

def start_bot():
    rsp = post("/api/bot/start", body={}, headers={ "Authorization" : "Bearer " + jwt })
    print_bot_message(rsp)

def message_bot():
    message = input("<<<\t")
    rsp = post("/api/bot/message", body={ "message": message }, headers={ "Authorization" : "Bearer " + jwt })
    print_bot_message(rsp)


def main():
    while True:
        try:
            command = input("signup, login, start, or exit:\t")
            if command == "signup":
                signup()
            elif command == "login":
                login()
            elif command == "exit":
                return
            elif command == "start":
                start_bot()
                try:
                    while True:
                        message_bot()
                except KeyboardInterrupt:
                    print("")
            else:
                print("unknow command:", command)
        except NoAuthorization:
            print("login required!")
        

if __name__ == '__main__':
    try:
        main()
        print("exit with exit command")
    except KeyboardInterrupt:
        print("exit with keyboard interrupt")
