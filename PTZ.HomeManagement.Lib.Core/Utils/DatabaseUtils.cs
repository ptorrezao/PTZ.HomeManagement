using System;
using Microsoft.Extensions.Configuration;
using PTZ.HomeManagement.Enums;

namespace PTZ.HomeManagement.Utils
{
    public static class DatabaseUtils
    {
        public static string GetConnectionString(IConfiguration configuration, DatabaseType databaseType)
        {
            var hostname = Environment.GetEnvironmentVariable("DB_HOST");
            var dbpassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "PTZHomeManagement";

            var connString = "";
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    connString = string.IsNullOrEmpty(hostname) ? configuration.GetConnectionString("DefaultConnection") : $"Data Source={hostname};Initial Catalog={dbName};User ID={dbUser};Password={dbpassword};";
                    break;
                case DatabaseType.PostgreSQL:
                case DatabaseType.CockroachDB:
                    connString = $"Host={hostname};Database={dbName};Username={dbUser};Password={dbpassword}";
                    break;
                case DatabaseType.SQLLite:
                    connString = $"{dbName}";
                    break;
                default:
                    break;
            }

            return connString;
        }
    }
}
