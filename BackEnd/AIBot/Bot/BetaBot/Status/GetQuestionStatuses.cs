using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Bot.BetaBot.Status
{

    public class WelcomeStatus : IBotStatusBehaviour<StatusId>
    {
        public StatusId Id => StatusId.Welcome;

        public Task EnterAsync(IBotStatusContainer status, IBotEnterContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<StatusId> ExitAsync(IBotStatusContainer status, IBotExitContext context)
        {
            var worker = context.Worker;
            var msg = context.Receiver.UserMessage;
            var res = await worker.GetLocalSearchResultAsync(msg);
            if (msg.Length <= AIBot.Constants.QuestionTitleMaxLength)
            {
                status.Put(Constants.Key.Checking, Constants.Value.CheckingShort);
                status.Put(Constants.Key.ShortQuestion, res);
            }
            else
            {
                status.Put(Constants.Key.Checking, Constants.Value.CheckingLong);
                status.Put(Constants.Key.LongQuestion, res);
            }
            var questions = status.GetSortedSelectedQuestions();
            return Id;
        }
    }
}
