using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(PresentationActionFilter));
                options.Filters.Add(typeof(RequiredPasswordChangeActionFilter));
                options.Filters.Add(new AuthorizeFilter(policy));
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

            app.UseStaticFiles();

            app.UseAuthentication();

            PrepareRoutes(app);

            PrepareLoggers(loggerFactory);
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
