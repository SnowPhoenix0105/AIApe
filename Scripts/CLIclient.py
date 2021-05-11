# -*- coding: utf8 -*-

import json
import urllib.request
from utils.Poster import post
from utils.Poster import login
from utils.Poster import signup
from utils.Poster import NoAuthorization



def print_bot_message(rsp):
    messages = rsp["messages"]
    for message in messages:
        message_lines = message.strip().replace("\\[", "[").replace("\\]", "]").split("\n")
        print(">>>\t" + "\n\t".join(message_lines))
    prompt = rsp["prompt"]
    if len(prompt) != 0:
        print(prompt)

def start_bot():
    rsp = post("/api/bot/start", body={})
    print_bot_message(rsp)

def message_bot():
    message = input("<<<\t")
    rsp = post("/api/bot/message", body={ "message": message })
    print_bot_message(rsp)


def main():
    while True:
        command = input("signup, login, start, or exit:\t")
        try:
            if command == "signup":
                signup()
            elif command == "login":
                login()
            elif command == "exit":
                return
            elif command == "start":
                start_bot()
                while True:
                    message_bot()
            else:
                print("unknow command:", command)
        except KeyboardInterrupt:
            print("")
        except NoAuthorization:
            print("login required!")
        

if __name__ == '__main__':
    try:
        main()
        print("exit with exit command")
    except KeyboardInterrupt:
        print("\nexit with keyboard interrupt")
