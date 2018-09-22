using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Models;

namespace PTZ.HomeManagement.Core.Data
{
    public class ApplicationDbRepository : IApplicationRepository
    {
        public static Dictionary<string, string> DefaultUsers => ApplicationDbContext.DefaultUsers;

        public static List<string> DefaultAdmins => ApplicationDbContext.DefaultAdmins;

        private readonly ApplicationDbContext context;

        public ApplicationDbRepository(IServiceProvider serviceProvider)
        {
            try
            {
                DbContextOptions<ApplicationDbContext> options = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
                this.context = new ApplicationDbContext(options);

                if (!options.Extensions.Any(x => x.GetType() == typeof(InMemoryOptionsExtension)) &&
                     (!this.context.Database.GetAppliedMigrations().Any(x => x == this.context.Database.GetMigrations().LastOrDefault())))
                {
                    this.context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbRepository>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        public ApplicationUser GetUser(string userId)
        {
            return context.Users.FirstOrDefault(x => string.IsNullOrEmpty(userId) || x.Id == userId);
        }
        public IQueryable<ApplicationUser> GetUsers(string userId = null)
        {
            return context.Users.Where(x => string.IsNullOrEmpty(userId) || x.Id == userId);
        }

        public bool OnlyDefaultUserIsAvailable()
        {
            return context.Users.Count(x => !DefaultUsers.ContainsKey(x.UserName)) <= 0;
        }

        public void SaveUser(ApplicationUser user)
        {
            this.context.Entry(user).State = !this.context.Users.Any(x => x.Id == user.Id) ? EntityState.Added : EntityState.Modified;
            this.context.SaveChanges();
        }

        public string GetConfiguration(string configName)
        {
            var config = this.context.Configurations.FirstOrDefault(x => x.Name == configName);
            return config != null ? config.Value : "";
        }

    }
}
