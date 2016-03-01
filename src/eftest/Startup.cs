using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace eftest
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var application = new WebHostBuilder()
                .UseCaptureStartupErrors(captureStartupError: true)
                .UseDefaultConfiguration(args)
                .UseIISPlatformHandlerUrl()
                .UseServer("Microsoft.AspNetCore.Server.Kestrel")
                .UseStartup<Startup>()
                .Build();

            application.Run();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // add entity framework using the config connection string
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer("Server=localhost;Database=eftest;Trusted_Connection=True;MultipleActiveResultSets=true;"));

            //// configure mvc
            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                var jsonOutputFormatter = new JsonOutputFormatter();
                jsonOutputFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                jsonOutputFormatter.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
                options.OutputFormatters.Insert(0, jsonOutputFormatter);
            });

        }

        public void Configure(IApplicationBuilder app,
            //IDatabaseInitializer databaseInitializer,
            ILoggerFactory loggerFactory)
        {
            var factory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            factory.AddConsole(minLevel: LogLevel.Debug);
            factory.AddDebug(minLevel: LogLevel.Debug);

            app.UseDeveloperExceptionPage();

            app.UseIISPlatformHandler();

            // to serve up index.html
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            //databaseInitializer.Seed();
        }
    }
}
