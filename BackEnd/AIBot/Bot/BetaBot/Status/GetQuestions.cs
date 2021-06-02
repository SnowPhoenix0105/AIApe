using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.Framework;
using Buaa.AIBot.Bot.WorkingModule;
using Buaa.AIBot.Bot;


namespace Buaa.AIBot.Bot.BetaBot.Status
{
    public static class GetQuestions
    {
        public static async Task GetLocalSearchResultAsync(string content, IWorkingModule worker)
        {

        }
    }

    public class WelcomeStatus : IBotStatusBehaviour<StatusId>
    {
        public StatusId Id => StatusId.Welcome;

        public Task EnterAsync(IBotStatusContainer status, IBotEnterContext context)
        {
            throw new NotImplementedException();
        }

        public Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            var worker = context.Worker;
        }
    }
}
