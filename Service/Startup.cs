using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace CampusPulse.Trade.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var baseRoot = Directory.GetCurrentDirectory();
            var config = new ConfigurationBuilder()
                .SetBasePath(baseRoot)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                .AddEnvironmentVariables()

                .Build();
            Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Information()
                //.WriteTo.RollingFile("log-{Date}.txt", LogEventLevel.Information)
                .ReadFrom.Configuration(config)
                .CreateLogger();

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {

                ///options.UserWebcore();
            });
            services.Configure<MvcOptions>(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole(LogLevel.Debug);
            loggerFactory.AddDebug(LogLevel.Debug);
            loggerFactory.AddSerilog();
            // MVC need to be the last one
            //app.UseMvc(RouteHelper.AddRoutesToConfiguration);
            app.UseMvc(routes =>
            {
                routes.MapRoute("service-status", "service-status", defaults: new { controller = "Status", Action = "Get" });
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
