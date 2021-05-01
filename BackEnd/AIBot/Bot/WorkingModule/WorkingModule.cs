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
                .AddSingleton<GovernmentInstallingInfo>()
                ;
            return services;
        }
    }

    public interface IWorkingModule
    {
        QuestionBuilder GetQuestionBuilder();
        GovernmentInstallingInfo GetGovernmentInstallingInfo();
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

        public GovernmentInstallingInfo GetGovernmentInstallingInfo()
        {
            return services.GetService<GovernmentInstallingInfo>();
        }
    }
}
