// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EmptyBot .NET Template version v4.12.2

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Buaa.AIBot.BackEnd
{
     public class Program
    {
        public static readonly string LOG_FILE_PATH = "";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configureation) => 
                {
                    configureation
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        .WriteTo.Console()
                        .WriteTo.File(LOG_FILE_PATH);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging((logging) =>
                    {
                        logging.AddDebug();
                        logging.AddConsole();
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
