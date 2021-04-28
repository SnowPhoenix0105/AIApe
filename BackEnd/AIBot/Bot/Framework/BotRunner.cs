using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.Framework
{
    public class InputInfo
    {
        public string Message { get; set; } 
    }

    public class OutputInfo
    {
        public string Message { get; set; }
        public IEnumerable<string> Prompts { get; set; }
    }

    public interface IBotRunner
    {
        Task<OutputInfo> Run(int userId, InputInfo input);
        Task<OutputInfo> Start(int userId);
    }

    public class BotRunnerOptions<IdType>
    {
        public IStatusPool<IdType> StatusPool { get; set; }
        public IStatusBehaviourPool<IdType> BehaviourPool { get; set; }
        public BotStatus<IdType> InitStatus { get; set; }
    }

    public class BotRunner<IdType> : IBotRunner
    {
        BotRunnerOptions<IdType> options;

        private IStatusPool<IdType> StatusPool => options.StatusPool;
        private IStatusBehaviourPool<IdType> BehaviourPool => options.BehaviourPool;

        public BotRunner(BotRunnerOptions<IdType> options)
        {
            this.options = options;
        }

        public async Task<OutputInfo> Start(int userId)
        {
            var origin = options.InitStatus;
            var status = new BotStatus<IdType>()
            {
                Status = origin.Status
            };
            foreach (var pair in origin.Items)
            {
                status.Items.Add(pair.Key, pair.Value);
            }
            var sender = new Sender();
            var nextStatusBehaviour = BehaviourPool[status.Status];
            await nextStatusBehaviour.EnterAsync(status, sender);
            return sender.DumpToOutputInfo();
        }

        public async Task<OutputInfo> Run(int userId, InputInfo input)
        {
            var receiver = new InputReceiverAdapter()
            {
                Input = input
            };
            var sender = new Sender();
            var status = await StatusPool.GetStatusAsync(userId);
            var currentStatusBehaviour = BehaviourPool[status.Status];
            status.Status = await currentStatusBehaviour.ExitAsync(status, sender, receiver);
            var nextStatusBehaviour = BehaviourPool[status.Status];
            await nextStatusBehaviour.EnterAsync(status, sender);
            return sender.DumpToOutputInfo();
        }

        private class InputReceiverAdapter : IBotReceiver
        {
            public string UserMessage => Input.Message;
            public InputInfo Input { get; set; }
        }

        private class Sender : IBotSender
        {
            public List<string> Messages { get; } = new List<string>();
            public List<string> Prompts { get; } = new List<string>();

            public void AddMessage(string message)
            {
                Messages.Add(message);
            }

            public IBotSender AddPrompt(string prompt)
            {
                Prompts.Add(prompt);
                return this;
            }

            public OutputInfo DumpToOutputInfo()
            {
                return new OutputInfo()
                {
                    Message = string.Join("\n", Messages),
                    Prompts = Prompts
                };
            }
        }
    }
}
