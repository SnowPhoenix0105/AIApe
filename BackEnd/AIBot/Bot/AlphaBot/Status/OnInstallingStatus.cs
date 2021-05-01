using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Bot.AlphaBot.Status
{
    public class GetOSForInstallingStatus : IBotStatusBehaviour<StatusId>
    {
        public StatusId Id => StatusId.GetOSForInstalling;

        public Task EnterAsync(IBotStatusContainer status, IBotEnterContext context)
        {
            var sender = context.Sender;
            sender
                .AddMessage("请问您的操作系统是什么呢？")
                .AddPrompt(Value.WindowsOS).AddPrompt(Value.LinuxOS).AddPrompt(Value.MaxOS);
            return Task.CompletedTask;
        }

        private bool IsWindows(string msg)
        {
            return msg.ToLowerInvariant().Contains(Value.WindowsOS.ToLowerInvariant())
                || msg.ToLowerInvariant().Contains("win");
        }

        private bool IsMac(string msg)
        {
            return msg.ToLowerInvariant().Contains(Value.MaxOS.ToLowerInvariant())
                || msg.ToLowerInvariant().Contains("mac")
                || msg.ToLowerInvariant().Contains("apple");
        }

        private bool IsLinux(string msg)
        {
            var list = new List<string>
            {
                "Linux", "ubuntu", "Redhat", "Debain", "Fedora", "openSUSE", "Mandriva", "Mint",
                "PCLinuxOS", "Cent OS", "CentOS", "Slackware", "Gentoo"
            };
            list.Add(Value.LinuxOS);
            foreach (var linux in list)
            {
                if (msg.ToLowerInvariant().Contains(linux.ToLowerInvariant()))
                {
                    return true;
                }
            }
            return false;
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            string msg = context.Receiver.UserMessage;
            if (IsWindows(msg))
            {
                status.Put(Key.OS, Value.WindowsOS);
            }
            else if (IsMac(msg))
            {
                status.Put(Key.OS, Value.MaxOS);
            }
            else if (IsLinux(msg))
            {
                status.Put(Key.OS, Value.LinuxOS);
            }
            else
            {
                context.Sender.AddMessage($"抱歉，我不认识你说的操作系统{Kaomojis.Sad}").NewScope();
                return Task.FromResult(Id);
            }
            status.Put(Key.OS_detail, msg);
            return Task.FromResult(StatusId.GetIDEForInstalling);
        }
    }

    public class GetIDEForInstallingStatus : IBotStatusBehaviour<StatusId>
    {
        public StatusId Id => StatusId.GetIDEForInstalling;

        public Task EnterAsync(IBotStatusContainer status, IBotEnterContext context)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
