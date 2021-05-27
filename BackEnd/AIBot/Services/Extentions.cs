using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Services.CodeAnalyze;

namespace Buaa.AIBot.Services
{
    public static class Extentions
    {
        public static IServiceCollection AddCodeAnalyze(this IServiceCollection services, IConfiguration config)
        {
            string cppcheckWorkDir = config.GetSection("Path").GetValue<string>("CppCheckWorkDir");
            services
                .AddTransient<ICodeAnalyzeService, CodeAnalyzeService>()
                .AddSingleton<CppCheckResultTanslation>()
                .AddSingleton<ICppCheckCallerFactory>(provider =>
                    new CppCheckCallerFactory(cppcheckWorkDir, provider.GetRequiredService<ILogger<CppCheckCallerFactory>>()))
                ;
            return services;
        }
    }
}
