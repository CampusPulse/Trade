using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CampusPulse.Trade.Service;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var baseRoot = Directory.GetCurrentDirectory();
            Console.WriteLine(baseRoot);
            Startup.BasePath = baseRoot;

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

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(baseRoot)
                .UseKestrel(options =>
                {
                    options.ConfigureEndpoints();
                })
                .UseConfiguration(config)
                .Build();
        }
    }
}
