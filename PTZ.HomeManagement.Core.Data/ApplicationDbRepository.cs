using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Models;

namespace PTZ.HomeManagement.Core.Data
{
    public class ApplicationDbRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationDbRepository(IServiceProvider serviceProvider)
        {
            DbContextOptions<ApplicationDbContext> options = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            this.context = new ApplicationDbContext(options);

            if (!options.Extensions.Any(x => x.GetType() == typeof(InMemoryOptionsExtension)))
            {
                var lastDefinedMigration = this.context.Database.GetMigrations().LastOrDefault();
                if (this.context.Database.GetAppliedMigrations().Any(x => x == lastDefinedMigration))
                {
                    this.context.Database.Migrate();
                }
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

        public void SaveUser(ApplicationUser user)
        {
            this.context.Entry(user).State = !this.context.Users.Any(x => x.Id == user.Id) ? EntityState.Added : EntityState.Modified;
            this.context.SaveChanges();
        }
    }
}
