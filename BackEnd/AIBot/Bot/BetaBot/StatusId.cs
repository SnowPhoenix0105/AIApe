using Buaa.AIBot.Bot.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.BetaBot
{
    public enum StatusId
    {
        Welcome,
        GetLonger,
        GetShorter,
            ShowLimitResult,
            ReduceResultByTags,
                    ShowAllResult,
                        CreateAnswer,
    }

    public static class Configuration
    {
        public static Dictionary<StatusId, IBotStatusBehaviour<StatusId>> GetStatusBehaviours()
        {
            var list = new List<IBotStatusBehaviour<StatusId>>()
            {
            };
            var ret = new Dictionary<StatusId, IBotStatusBehaviour<StatusId>>();
            foreach (var status in list)
            {
                ret[status.Id] = status;
            }
            return ret;
        }
    }

}
