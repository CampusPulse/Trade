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
using Microsoft.Extensions.Logging;
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
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)

           .UseKestrel(options =>
           {
               options.Listen(IPAddress.Any, 443);
           });
            // Local debug using Local Https profile
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                webHostBuilder.UseUrls("http://local.dev.campuspulse.com:443");
            }

            return webHostBuilder.UseStartup<Startup>().Build();
        }
    }
}
