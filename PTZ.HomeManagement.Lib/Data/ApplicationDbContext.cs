using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PTZ.HomeManagement.Models;
using System;

namespace PTZ.HomeManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static string GetConnectionString(IConfiguration Configuration)
        {
            var hostname = Environment.GetEnvironmentVariable("SQLSERVER_HOST");
            var dbpassword = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("SQLSERVER_USERNAME") ?? "sa";
            var dbName = Environment.GetEnvironmentVariable("SQLSERVER_DB_NAME") ?? "PTZHomeManagement";

            var connString = "";
            if (string.IsNullOrEmpty(hostname))
            {
                connString = Configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                connString = $"Data Source={hostname};Initial Catalog={dbName};User ID={dbUser};Password={dbpassword};";
            }

            return connString;
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
