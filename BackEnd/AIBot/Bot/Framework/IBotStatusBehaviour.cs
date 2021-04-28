using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.Framework
{
    public interface IBotSender
    {
        void AddMessage(string message);
        IBotSender AddPrompt(string prompt);
    }

    public interface IBotReceiver
    {
        string UserMessage { get; }
    }

    public interface IBotStatusBehaviour<IdType>
    {
        IdType Id { get; }
        Task EnterAsync(IBotStatus<IdType> status, IBotSender sender);
        Task<IdType> ExitAsync(IBotStatus<IdType> status, IBotSender sender, IBotReceiver receiver);
    }
}
