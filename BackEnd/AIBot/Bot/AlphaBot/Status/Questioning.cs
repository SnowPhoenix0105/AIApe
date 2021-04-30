using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Bot.AlphaBot.Status
{
    public class GetSimpleDescribeStatus : IBotStatusBehaviour<StatusId>
    {
        private static readonly int minLength = 5;

        public StatusId Id => StatusId.GetSimpleDescribe;

        public Task EnterAsync(IBotStatusContainer status, IBotEnterContext context)
        {
            var sender = context.Sender;
            sender.AddMessage($"可以简单描述一下你的问题嘛（{Constants.QuestionTitleMaxLength}字以内）{Kaomojis.Cute}");
            return Task.CompletedTask;
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            var receiver = context.Receiver;
            var sender = context.Sender;
            string msg = receiver.UserMessage;
            if (msg.Length > Constants.QuestionTitleMaxLength)
            {
                sender
                    .AddMessage($"问题太长啦{Kaomojis.Sad}")
                    .AddMessage($"详细描述我过一会儿再问你")
                    .AddMessage($"不能多于{Constants.QuestionTitleMaxLength}个字哟")
                    .NewScope();
                return Task.FromResult(Id);
            }
            if (msg.Length < minLength)
            {
                sender
                    .AddMessage($"问题太短啦{Kaomojis.Sad}")
                    .AddMessage($"至少也得有{minLength}个字吧？")
                    .NewScope();
                return Task.FromResult(Id);
            }
            status.Put(Key.SimpleDescribe, msg);
            // TODO
            return Task.FromResult(StatusId.GetDetails);
        }
    }


    public class GetDetailsStatus : IBotStatusBehaviour<StatusId>
    {
        private static readonly int minLength = 5;
        public StatusId Id => StatusId.GetDetails;

        public Task EnterAsync(IBotStatusContainer status, IBotEnterContext context)
        {
            var sender = context.Sender;
            sender.AddMessage($"详细描述一下你的问题吧（字数不限）{Kaomojis.Cute}");
            return Task.CompletedTask;
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            var receiver = context.Receiver;
            var sender = context.Sender;
            string msg = receiver.UserMessage;
            if (msg.Length < minLength)
            {
                sender
                    .AddMessage($"你的“详细描述”太短啦{Kaomojis.Sad}")
                    .AddMessage($"至少也得有{minLength}个字吧？")
                    .NewScope();
                return Task.FromResult(Id);
            }
            status.Put(Key.DetailDescribe, msg);
            // TODO
            return Task.FromResult(StatusId.AddQuestion);
        }
    }

    public class AddQuestionStatus : IBotStatusBehaviour<StatusId>
    {
        public StatusId Id => StatusId.AddQuestion;

        public async Task EnterAsync(IBotStatusContainer status, IBotEnterContext context)
        {
            var sender = context.Sender;
            var builder = context.Worker.GetQuestionBuilder();
            var qid = await builder.BuildAsync(status);
            sender
                .AddMessage($"小猿已经帮你创建了提问咯{Kaomojis.Happy}")
                .AddMessage("请耐心等待人工回答~")
                .AddMessage("你可以在", false).AddQuestion(qid).AddMessage($"查看这个问题哟{Kaomojis.Cute}")
                .AddPrompt("好哒")
                ;
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            return Task.FromResult(StatusId.Welcome);
        }
    }
}
