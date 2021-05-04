using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.WorkingModule
{
    public static class Configuration
    {
        public static IServiceCollection AddWorkingModule(this IServiceCollection services, IConfiguration config)
        {
            string gccWorkDir = config.GetSection("Path").GetValue<string>("GccWorkDir");
            services
                .AddTransient<IWorkingModule, WorkingModule>()
                .AddTransient<QuestionBuilder>()
                .AddSingleton<GovernmentInstallingInfo>()
                .AddSingleton<IdeAndCompilerDocumentCollection>()
                .AddSingleton(provider => 
                    new GccHandlerFactory(gccWorkDir, provider.GetRequiredService<ILogger<GccHandlerFactory>>()))
                .AddSingleton<SourceCodeAnalyzer>()
                ;
            return services;
        }
    }

    public interface IWorkingModule
    {
        QuestionBuilder GetQuestionBuilder();
        GovernmentInstallingInfo GetGovernmentInstallingInfo();
        IdeAndCompilerDocumentCollection GetDocumentCollection();
        GccHandlerFactory GetGccHandlerFactory();
        SourceCodeAnalyzer GetSourceCodeAnalyzer();
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

        public IdeAndCompilerDocumentCollection GetDocumentCollection()
        {
            return services.GetService<IdeAndCompilerDocumentCollection>();
        }

        public GccHandlerFactory GetGccHandlerFactory()
        {
            return services.GetService<GccHandlerFactory>();
        }

        public SourceCodeAnalyzer GetSourceCodeAnalyzer()
        {
            return services.GetService<SourceCodeAnalyzer>();
        }
    }
}
