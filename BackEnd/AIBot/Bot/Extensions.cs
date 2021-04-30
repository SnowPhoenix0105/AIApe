using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Bot
{
    public static class Extensions
    {
        public static IServiceCollection AddEchoBot(this IServiceCollection services)
        {
            var options = new BotRunnerOptions<EchoBot.StatusEnum>()
            {
                StatusPool = new StatusContainerPoolInMemory<EchoBot.StatusEnum>(),
                BehaviourPool = new StatusBehaviourPool<EchoBot.StatusEnum>(EchoBot.GetStatusBehaviours()),
                InitStatus = new BotStatus<EchoBot.StatusEnum>()
                {
                    Status = EchoBot.StatusEnum.Welcome
                }
            };
            var bot = new BotRunner<EchoBot.StatusEnum>(options);
            services.AddSingleton<IBotRunner>(bot);

            return services;
        }
    }
}
