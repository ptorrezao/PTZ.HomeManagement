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
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Extentions;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Services;
using PTZ.HomeManagement.Utils;

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

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(PresentationActionFilter));
                options.Filters.Add(typeof(RequiredPasswordChangeActionFilter));
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(ModulesNames));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(ApplicationDbContext.GetConnectionString(Configuration)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ICoreService, CoreService>();
            services.AddTransient<IEmailSender, EmailSender>();
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
                app.UseExceptionHandler("/Control/Error");
                app.UseStatusCodePagesWithReExecute("/Control/Error", "?statusCode={0}");
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
                new CultureInfo("en-US"),
                new CultureInfo("pt-PT")
            };

            var requestLocationOptions = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
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
        }
    }
}
