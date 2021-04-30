using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Buaa.AIBot.Bot.Framework;
using Buaa.AIBot.Bot.WorkingModule;


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
            services
                .AddWorkingModule()
                .AddTransient<IBotRunner>(
                services => new BotRunner<EchoBot.StatusEnum>(options, services.GetService<IWorkingModule>()));

            return services;
        }

        public static IServiceCollection AddAlphaBot(this IServiceCollection services)
        {
            var options = new BotRunnerOptions<AlphaBot.StatusId>()
            {
                StatusPool = new StatusContainerPoolInMemory<AlphaBot.StatusId>(),
                BehaviourPool = new StatusBehaviourPool<AlphaBot.StatusId>(AlphaBot.Configuration.GetStatusBehaviours()),
                InitStatus = new BotStatus<AlphaBot.StatusId>()
                {
                    Status = AlphaBot.StatusId.Welcome
                }
            };
            services
                .AddWorkingModule()
                .AddTransient<IBotRunner>(
                services => new BotRunner<AlphaBot.StatusId>(options, services.GetService<IWorkingModule>()));

            return services;
        }
    }
}
