using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Models.MyFinance;
using System;

namespace PTZ.HomeManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountMovement> BankAccountMovements { get; set; }

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
    }
}
