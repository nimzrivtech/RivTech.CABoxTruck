using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RivTech.CABoxTruck.WebService.Extensions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using NLog.Web;
using System;
using log4net;
using NLog;
using NLog.Extensions.Logging;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using RivTech.CABoxTruck.WebService.Service.BackgroundServices.InfrastructureSetup;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;
using Quartz.Impl;
using RivTech.CABoxTruck.WebService.Service.Services.BackgroundServices.JobFactories;
using System.Collections.Specialized;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;

namespace RivTech.CABoxTruck.WebService.Web
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var nlogger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                var host = await CreateHostBuilder(args).Build().MigrateDatabase();
                host.Run();
            }
            catch (Exception e)
            {
                //NLog: catch setup errors
                nlogger.Error(e, "Stopped program because of exception");
                // throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    var currentDirectory = Directory.GetCurrentDirectory();

                    config
                        .SetBasePath(currentDirectory)
                        .AddJsonFile("appsettings.json", false, true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);

                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                    if (args != null)
                        config.AddCommandLine(args);
                })

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    builder.AddLog4Net("log4net.config");

                    var nlogLoggingConfig = new NLogLoggingConfiguration(context.Configuration.GetSection("NLog"));
                    builder.AddNLogWeb(nlogLoggingConfig);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQuartz(q =>
                    {
                        q.UseJobFactory<BoxtruckJobFactory>();
                        //q.UseDedicatedThreadPool(20);
                    });
                    services.AddTransient<EndorsementProcessJob>();
                    services.AddTransient<IssuanceProcessJob>();
                    services.AddTransient<IssuanceDefaultProcessJob>();
                    services.AddTransient<ResetProcessJob>();
                    services.AddTransient<BoxtruckJobFactory>();
                    // ASP.NET Core hosting
                    services.AddQuartzHostedService(options =>
                    {
                        // when shutting down we want jobs to complete gracefully
                        options.WaitForJobsToComplete = true;
                    });

                    services.ConfigureOptions<EndorsementProcessJobSetup>();
                    services.ConfigureOptions<IssuanceProcessJobSetup>();
                    services.ConfigureOptions<IssuanceDefaultProcessJobSetup>();
                    services.ConfigureOptions<ResetProcessJobSetup>();
                });
    }
}
