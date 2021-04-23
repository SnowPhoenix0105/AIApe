using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace Buaa.AIBot.Utils.Logging
{
    public static class Extensions
    {
        public static IHostBuilder ConfigureLoggingWithSerilog(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                // Configure Logging with lib Serilog
                .UseSerilog((context, services, configuration) =>
                {
                    var path = context.Configuration.GetSection("Path");
                    var logFileDir = Path.GetFullPath(path.GetValue<string>("LogFileDir"));
                    var logJsonDir = Path.GetFullPath(path.GetValue<string>("LogJsonDir"));
                    Log.Information("Saving Log file in {LogFileDir}", logFileDir);
                    Log.Information("Saving Log json in {LogJsonDir}", logJsonDir);
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        // Enable LogContext.PushProperty()
                        .Enrich.FromLogContext()
                        // Set default logLevel (cover appsetting.json)
                        .MinimumLevel.Information()

                        // Print log message to Console
                        .WriteTo.Logger(subLogger =>
                        {
                            // Logging exclude List
                            var disableClasses = new HashSet<string>()
                            {
                                "Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor",
                                "Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker",
                                "Microsoft.AspNetCore.Routing.EndpointMiddleware",
                            };
                            subLogger
                                // The min level to print
                                .MinimumLevel.Information()
                                // Exclude log-info from certain Classes
                                .Filter.ByExcluding(Matching.WithProperty<string>("SourceContext", t => disableClasses.Contains(t)))
                                .WriteTo.Console();
                        })

                        // Write log message to logfile
                        .WriteTo.Logger(subLogger =>
                        {
                            // Logging exclude List
                            var disableClasses = new HashSet<string>()
                            {
                                "Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor",
                                "Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker",
                                "Microsoft.AspNetCore.Routing.EndpointMiddleware",
                            };
                            subLogger
                                // The min level write to logfile
                                .MinimumLevel.Debug()
                                // Exclude log-info from certain Classes
                                .Filter.ByExcluding(Matching.WithProperty<string>("SourceContext", t => disableClasses.Contains(t)))
                                .WriteTo.File(
                                    // Reading path of logfile from configuration
                                    path: Path.Combine(logFileDir, ".log"),
                                    // allow buffering to reduce disk writting
                                    buffered: true,
                                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{RequestId}] {Message:lj}{NewLine}{Exception}",
                                    // flush buffer each second
                                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                                    // create a new logfile each day
                                    rollingInterval: RollingInterval.Day,
                                    // when logfile reach 4 MiB, create a new logfile
                                    fileSizeLimitBytes: 4 * 1024 * 1024,
                                    rollOnFileSizeLimit: true
                                    )
                                    ;
                        })

                        // Write full log message to jsonfile
                        .WriteTo.File(
                            formatter: new Serilog.Formatting.Compact.CompactJsonFormatter(),
                            // Reading path of logfile from configuration
                            path: Path.Combine(logFileDir, ".json"),
                            // The min level write to logfile
                            restrictedToMinimumLevel: LogEventLevel.Debug,
                            // allow buffering to reduce disk writting
                            buffered: true,
                            // flush buffer every 10 second
                            flushToDiskInterval: TimeSpan.FromSeconds(10),
                            // create a new logfile each day
                            rollingInterval: RollingInterval.Day,
                            // when logfile reach 4 MiB, create a new logfile
                            fileSizeLimitBytes: 4 * 1024 * 1024,
                            rollOnFileSizeLimit: true
                        )

                        // Write full log message to debugger
                        .WriteTo.Debug(
                            restrictedToMinimumLevel: LogEventLevel.Debug
                            )
                        ;
                });
        }
    }
}
