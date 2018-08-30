using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.Core.Data;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PTZ.HomeManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static Dictionary<string, string> DefaultUsers => new Dictionary<string, string> {
            { "admin@hmptz.local", "Ch4ng3_Th1s"}
        };

        public static List<string> DefaultAdmins => new List<string> {
            { "admin@hmptz.local"}
        };

        public DbSet<KeyValuesCollection> KeyCollections { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.Ignore(x => x.FullName);
            });

            builder.Entity<Configuration>(b =>
            {
                b.HasKey(x => x.Name);
            });
        }
    }
}
