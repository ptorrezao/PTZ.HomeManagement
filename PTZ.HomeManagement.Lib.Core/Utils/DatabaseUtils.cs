using System;
using Microsoft.Extensions.Configuration;

namespace PTZ.HomeManagement.Utils
{
    public static class DatabaseUtils
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var hostname = Environment.GetEnvironmentVariable("DB_HOST");
            var dbpassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "PTZHomeManagement";

            var connString = "";
            if (string.IsNullOrEmpty(hostname))
            {
                connString = configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                connString = $"Data Source={hostname};Initial Catalog={dbName};User ID={dbUser};Password={dbpassword};";
            }

            return connString;
        }
    }
}
