using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PTZ.HomeManagement.Utils
{
    public static class DatabaseUtils
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var hostname = Environment.GetEnvironmentVariable("SQLSERVER_HOST");
            var dbpassword = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("SQLSERVER_USERNAME") ?? "sa";
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
