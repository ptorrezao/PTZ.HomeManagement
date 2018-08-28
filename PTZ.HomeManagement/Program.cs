using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.MyFinance.Data;
using PTZ.HomeManagement.Utils;
using Sentry;

namespace PTZ.HomeManagement
{
    public class Program
    {
        protected Program()
        {
            Program.Main(null);
        }

        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                MigrateIfNecessary<ApplicationDbContext>(services);
                MigrateIfNecessary<MyFinanceDbContext>(services);

                SeedData.Initialize(services).Wait();
            }

            host.Run();
        }

        private static void MigrateIfNecessary<T>(IServiceProvider services) where T : DbContext
        {
            try
            {
                var context = services.GetRequiredService<T>();
                context.Database.EnsureCreated();

                var lastDefinedMigration = context.Database.GetMigrations().LastOrDefault();
                if (!context.Database.GetAppliedMigrations().Any(x => x == lastDefinedMigration))
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseNLog();

            string sentryDSN;
            CoreUtils.GetConfigFromEnviromentVariable("Sentry_DSN", string.Empty, out sentryDSN);

            if (!string.IsNullOrEmpty(sentryDSN))
            {
                webHost.UseSentry(opt =>
                {
                    opt.Init(i =>
                    {
                        bool sentryIncludeRequestPayload = true;
                        bool sentryIncludeActivityData = true;
                        LogLevel sentryMinimumBreadcrumbLevel;
                        LogLevel sentryMinimumEventLevel;

                        CoreUtils.GetConfigFromEnviromentVariable("Sentry_IncludeRequestPayload", true, out sentryIncludeRequestPayload);
                        CoreUtils.GetConfigFromEnviromentVariable("Sentry_IncludeActivityData", true, out sentryIncludeActivityData);
                        CoreUtils.GetConfigFromEnviromentVariable("Sentry_MinimumBreadcrumbLevel", LogLevel.Error, out sentryMinimumBreadcrumbLevel);
                        CoreUtils.GetConfigFromEnviromentVariable("Sentry_MinimumEventLevel", LogLevel.Error, out sentryMinimumEventLevel);
                        opt.Dsn = sentryDSN;
                        opt.IncludeRequestPayload = sentryIncludeRequestPayload;
                        opt.IncludeActivityData = sentryIncludeActivityData;
                        opt.Logging = new Sentry.AspNetCore.LoggingOptions()
                        {
                            MinimumBreadcrumbLevel = sentryMinimumBreadcrumbLevel,
                            MinimumEventLevel = sentryMinimumEventLevel,
                        };
                        opt.Release = typeof(Startup).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                        i.AddInAppExclude("Full");
                    });
                });
            }

            return webHost.Build();
        }

    }
}
