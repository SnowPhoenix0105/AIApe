// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EmptyBot .NET Template version v4.12.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Buaa.AIBot.Utils.Logging;
using Serilog;

namespace Buaa.AIBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                // .AddNewtonsoftJson()
                ;

            services
                // Create HttpContextAccessor to access HttpContext at any time
                .AddTransient<IHttpContextAccessor, HttpContextAccessor>()

                // Create IpAddressRecorder.
                .AddTransient<IpAddressRecorder>()
                
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                // Use serilog request-logging instead of origin.
                .UseSerilogRequestLogging()

                // .UseHttpsRedirection()

                .UseRouting()

                // .UseAuthorization()

                // Record IP Address of the requester.
                .UseIpAddressRecord()

                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
