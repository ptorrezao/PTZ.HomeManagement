using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.Core.Data;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Utils;
using System;
using System.Linq;

namespace PTZ.HomeManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<KeyValuesCollection> KeyCollections { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            if (!options.Extensions.Any(x => x.GetType() == typeof(InMemoryOptionsExtension)))
            {
                var lastDefinedMigration = this.Database.GetMigrations().LastOrDefault();
                if (!this.Database.GetAppliedMigrations().Any(x => x == lastDefinedMigration))
                {
                    this.Database.Migrate();
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.Ignore(x => x.FullName);
            });
        }
    }
}
