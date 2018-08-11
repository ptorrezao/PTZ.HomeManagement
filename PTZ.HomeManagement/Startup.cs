using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using PTZ.HomeManagement.Core.Data;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Enums;
using PTZ.HomeManagement.Extentions;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.MyFinance.Data;
using PTZ.HomeManagement.Services;
using PTZ.HomeManagement.Utils;
using Sentry.Extensibility;

namespace PTZ.HomeManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            env.ConfigureNLog("nlog.config");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var policy = new AuthorizationPolicyBuilder()
               .RequireAuthenticatedUser()
               .Build();

            services.Configure<EmailSettings>(x=>
            {
                x.ApiKey = Environment.GetEnvironmentVariable("MailGun_ApiKey") ?? "";
                x.ApiBaseUri = Environment.GetEnvironmentVariable("MailGun_ApiBaseUri") ?? "";
                x.RequestUri = Environment.GetEnvironmentVariable("MailGun_RequestUri") ?? "";
                x.From = Environment.GetEnvironmentVariable("MailGun_From") ?? "";
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(PresentationActionFilterAttribute));
                options.Filters.Add(typeof(RequiredPasswordChangeActionFilterAttribute));
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();

            services.AddDbContext<ApplicationDbContext>(options => this.SetCorrectProvider(options));
            services.AddDbContext<MyFinanceDbContext>(options => this.SetCorrectProvider(options));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ICoreService, CoreService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IApplicationRepository, ApplicationDbRepository>();

            services.AddTransient<IMyFinanceRepository, MyFinanceRepositoryEF>();
            services.AddTransient<IMyFinanceService, MyFinanceService>();

            services.AddSingleton<ISentryEventExceptionProcessor, CustomSentryEventExceptionProcessor>();
            services.AddTransient<ISentryEventProcessor, CustomSentryEventEventProcessor>();
        }

        private void SetCorrectProvider(DbContextOptionsBuilder options)
        {
            string envVar = Environment.GetEnvironmentVariable("DB_TYPE");
            DatabaseType dbType;
            Enum.TryParse(envVar ?? DatabaseUtils.GetDefaultDb(), out dbType);

            string connectionString = DatabaseUtils.GetConnectionString(Configuration, dbType);

            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    options.UseSqlServer(connectionString);
                    break;
                case DatabaseType.PostgreSQL:
                    options.UseNpgsql(connectionString);
                    break;
                default:
                    options.UseSqlite(connectionString);
                    break;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                var errorManagement = string.Format("/{0}/{1}", "Control", nameof(Controllers.ControlController.Error));
                app.UseExceptionHandler(errorManagement);
                app.UseStatusCodePagesWithReExecute(errorManagement, "?statusCode={0}");
            }

            PrepareLocalization(app);

            app.UseStaticFiles();

            app.UseAuthentication();

            PrepareRoutes(app);

            PrepareLoggers(loggerFactory);
        }

        private static void PrepareLocalization(IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("pt-PT"),
            };

            var requestLocationOptions = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-PT"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };

            app.UseRequestLocalization(requestLocationOptions);
        }

        private static void PrepareRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Control}/{action=Index}/{id?}");
            });
        }

        private static void PrepareLoggers(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            loggerFactory.AddConsole();
        }
    }
}
