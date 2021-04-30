using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.WorkingModule
{
    public static class Configuration
    {
        public static IServiceCollection AddWorkingModule(this IServiceCollection services)
        {
            services
                .AddTransient<IWorkingModule, WorkingModule>()
                .AddTransient<QuestionBuilder>()
                ;
            return services;
        }
    }

    public interface IWorkingModule
    {
        QuestionBuilder GetQuestionBuilder();
    }

    public class WorkingModule : IWorkingModule
    {
        IServiceProvider services;

        public WorkingModule(IServiceProvider services)
        {
            this.services = services;
        }

        public QuestionBuilder GetQuestionBuilder()
        {
            return services.GetService<QuestionBuilder>();
        }
    }
}
