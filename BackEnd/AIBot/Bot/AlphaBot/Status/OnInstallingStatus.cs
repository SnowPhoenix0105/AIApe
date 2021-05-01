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
                .AddPrompt(Value.WindowsOS).AddPrompt(Value.LinuxOS).AddPrompt(Value.MacOS);
            return Task.CompletedTask;
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            string msg = context.Receiver.UserMessage;
            if (msg.ToLowerContains(Value.WindowsOS, "win"))
            {
                status.Put(Key.OS, Value.WindowsOS);
            }
            else if (msg.ToLowerContains(Value.MacOS, "mac", "apple"))
            {
                status.Put(Key.OS, Value.MacOS);
            }
            else if (msg.ToLowerContains(
                Value.LinuxOS, "Linux", 
                "ubuntu", "Redhat", "Debain", "Fedora", "openSUSE", "Mandriva", "Mint",
                "PCLinuxOS", "Cent OS", "CentOS", "Slackware", "Gentoo"))
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
            var sender = context.Sender;
            sender
                .AddMessage("请问你想安装的IDE/编译器是什么呢？")
                .AddMessage(Value.DevCpp).AddPrompt(Value.VisualCpp).AddPrompt(Value.VS).AddPrompt(Value.VSCode)
                ;
            return Task.CompletedTask;
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            string msg = context.Receiver.UserMessage;
            if (msg.ToLowerContains(Value.DevCpp, "dev"))
            {
                status.Put(Key.IDE, Value.DevCpp);
            }
            else if (msg.ToLowerContains(Value.VisualCpp, "visualc", "visual c", "VC++", "VC"))
            {
                status.Put(Key.IDE, Value.VisualCpp);
            }
            else if (msg.ToLowerContains(Value.VSCode, "code", "vs code", "vscode", "visualstudio code", "visualstudiocode", "visual studio code"))
            {
                status.Put(Key.IDE, Value.VSCode);
            }
            else if (msg.ToLowerContains(Value.VS, "vs", "visualstudio"))
            {
                status.Put(Key.IDE, Value.VS);
            }
            else
            {
                context.Sender.AddMessage($"抱歉，我不认识你说的IDE/编译器{Kaomojis.Sad}").NewScope();
                return Task.FromResult(Id);
            }
            status.Put(Key.IDE_detail, msg);
            return Task.FromResult(StatusId.GovernmentLinkForInstalling);
        }
    }

    public class GovernmentLinkForInstallingStatus : IBotStatusBehaviour<StatusId>
    {
        public StatusId Id => StatusId.GovernmentLinkForInstalling;

        public Task EnterAsync(IBotStatusContainer status, IBotEnterContext context)
        {
            context.Worker
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            throw new NotImplementedException();
        }
    }
}
