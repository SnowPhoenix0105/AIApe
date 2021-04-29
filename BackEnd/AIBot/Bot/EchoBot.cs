using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Bot
{
    public class EchoBot
    {
        private static readonly string MsgKey = "msg";
        private static readonly string RestartPrompt = "重新开始";

        public enum StatusEnum
        {
            Welcome, Echo
        }

        public static Dictionary<EchoBot.StatusEnum, IBotStatusBehaviour<EchoBot.StatusEnum>> GetStatusBehaviours()
        {
            var list = new List<IBotStatusBehaviour<StatusEnum>>() { new WelcomeStatus(), new EchoStatus() };
            var ret = new Dictionary<StatusEnum, IBotStatusBehaviour<StatusEnum>>();
            foreach (var status in list)
            {
                ret[status.Id] = status;
            }
            return ret;
        }

        private static StatusEnum Receive(IBotStatusContainer<StatusEnum> status, IBotSender sender, IBotReceiver receiver)
        {
            string newMsg = receiver.UserMessage;
            if (newMsg == RestartPrompt)
            {
                status.Put(MsgKey, new List<string>());
                sender.AddMessage("重新开始！\n");
                return StatusEnum.Welcome;
            }
            else
            {
                List<string> received = status.Get<List<string>>(MsgKey);
                received.Add(receiver.UserMessage);
                status.Put(MsgKey, received);
                return StatusEnum.Echo;
            }
        }

        public class WelcomeStatus : IBotStatusBehaviour<StatusEnum>
        {
            public StatusEnum Id => StatusEnum.Welcome;

            public Task EnterAsync(IBotStatusContainer<StatusEnum> status, IBotSender sender)
            {
                sender.AddMessage("你好，我叫小猿。\n");
                sender.AddMessage("我现还没有开发完成，我只会记住你说的话，然后将所有你说的话重复下来。");
                sender.AddMessage($"你可以随时输入“{RestartPrompt}”，来删除我的记录。");
                status.Put(MsgKey, new List<string>());
                return Task.CompletedTask;
            }

            public Task<StatusEnum> ExitAsync(IBotStatusContainer<StatusEnum> status, IBotSender sender, IBotReceiver receiver)
            {
                return Task.FromResult(Receive(status, sender, receiver));
            }
        }

        public class EchoStatus : IBotStatusBehaviour<StatusEnum>
        {

            public StatusEnum Id => StatusEnum.Echo;

            public Task EnterAsync(IBotStatusContainer<StatusEnum> status, IBotSender sender)
            {
                List<string> received = status.Get<List<string>>(MsgKey);
                if (received.Count == 0)
                {
                    sender.AddMessage("你现在还没有发任何消息");
                }
                else
                {
                    string msg = string.Join("]\n[", received);
                    sender.AddMessage($"你现在发过了{received.Count}条消息，它们是：\n[{msg}]");
                }
                sender.AddPrompt(RestartPrompt);
                return Task.FromResult(StatusEnum.Echo);
            }

            public Task<StatusEnum> ExitAsync(IBotStatusContainer<StatusEnum> status, IBotSender sender, IBotReceiver receiver)
            {
                return Task.FromResult(Receive(status, sender, receiver));
            }
        }
    }
}
