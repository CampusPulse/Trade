using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CampusPulse.Trade.Service
{
    public class Startup
    {
        public static string BasePath { get; internal set; }     

        public Startup(IHostingEnvironment env)
        {
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
                ////options.Filters.Add(new RequireHttpsAttribute());
            });

            /*services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("example.com");
                options.ExcludedHosts.Add("www.example.com");
            });
            */

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            int? httpsPort = null;
            var httpsSection = configuration.GetSection("HttpServer:Endpoints:Https");
            if (httpsSection.Exists())
            {
                var httpsEndpoint = new EndpointConfiguration();
                httpsSection.Bind(httpsEndpoint);
                httpsPort = httpsEndpoint.Port;
            }
            var statusCode = env.IsDevelopment() ? StatusCodes.Status302Found : StatusCodes.Status301MovedPermanently;

            app.UseRewriter(new RewriteOptions().AddRedirectToHttps(statusCode, httpsPort));

           
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
