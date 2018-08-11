using System;
using Microsoft.Extensions.Configuration;
using PTZ.HomeManagement.Enums;

namespace PTZ.HomeManagement.Utils
{
    public static class DatabaseUtils
    {
        public static string GetConnectionString(IConfiguration configuration, DatabaseType databaseType)
        {
            var hostname = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbpassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "PTZHomeManagement";
            var port = 0;
            var connString = "";
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    dbUser = dbUser ?? "sa";
                    connString = $"Data Source={hostname};Initial Catalog={dbName};User ID={dbUser};Password={dbpassword};";
                    break;
                case DatabaseType.PostgreSQL:
                    dbUser = dbUser ?? "postgres";
                    dbpassword = dbpassword ?? "Filipe#88";
                    port = 5432;
                    connString = $"Host={hostname};Port={port};Username={dbUser};Database={dbName};Password={dbpassword}";
                    break;
                case DatabaseType.SQLLite:
                    connString = $"Data Source={dbName}";
                    break;
                default:
                    break;
            }

            return connString;
        }

        public static string GetDefaultDb()
        {
            return DatabaseType.SQLLite.ToString();
        }
    }
}
