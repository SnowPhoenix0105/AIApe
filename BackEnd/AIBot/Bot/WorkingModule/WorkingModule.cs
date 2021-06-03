﻿using Microsoft.Extensions.Configuration;
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
            double minScore = config.GetSection("BetaBot").GetValue<double>("MinScore");
            services
                .AddTransient<IWorkingModule, WorkingModule>()
                .AddTransient<QuestionBuilder>()
                .AddSingleton<GovernmentInstallingInfo>()
                .AddSingleton<IdeAndCompilerDocumentCollection>()
                .AddSingleton<IGccHandlerFactory>(provider =>
                    new GccHandlerFactory(gccWorkDir, provider.GetRequiredService<ILogger<GccHandlerFactory>>()))
                .AddSingleton<SourceCodeAnalyzer>()
                .AddSingleton<DocumentCollection>()
                .AddTransient(provider =>
                    new InnerRepoSearcher(
                        provider.GetRequiredService<Services.IQuestionService>(),
                        provider.GetRequiredService<Services.INLPService>(),
                        minScore))
                .AddTransient<OuterRepoSearcher>()
                ;
            return services;
        }
    }

    public interface IWorkingModule
    {
        QuestionBuilder GetQuestionBuilder();
        GovernmentInstallingInfo GetGovernmentInstallingInfo();
        IdeAndCompilerDocumentCollection GetIdeAndCompilerDocumentCollection();
        IGccHandlerFactory GetGccHandlerFactory();
        SourceCodeAnalyzer GetSourceCodeAnalyzer();
        DocumentCollection GetDocumentCollection();
        OuterRepoSearcher GetOuterRepoSearcher();
        InnerRepoSearcher GetInnerRepoSearcher();
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

        public IdeAndCompilerDocumentCollection GetIdeAndCompilerDocumentCollection()
        {
            return services.GetService<IdeAndCompilerDocumentCollection>();
        }

        public IGccHandlerFactory GetGccHandlerFactory()
        {
            return services.GetService<IGccHandlerFactory>();
        }

        public SourceCodeAnalyzer GetSourceCodeAnalyzer()
        {
            return services.GetService<SourceCodeAnalyzer>();
        }

        public DocumentCollection GetDocumentCollection()
        {
            return services.GetService<DocumentCollection>();
        }

        public OuterRepoSearcher GetOuterRepoSearcher()
        {
            return services.GetService<OuterRepoSearcher>();
        }

        public InnerRepoSearcher GetInnerRepoSearcher()
        {
            return services.GetService<InnerRepoSearcher>();
        }
    }
}
