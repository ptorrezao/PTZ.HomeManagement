using Microsoft.EntityFrameworkCore;
using PTZ.HomeManagement.ExpirationReminder.Core;
using System;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core.EF
{
    public class ExpirationReminderDbContext : DbContext
    {
        public DbSet<Reminder> Reminders { get; set; }

        public ExpirationReminderDbContext(DbContextOptions<ExpirationReminderDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasSequence<long>("Reminder").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Reminder>(b =>
            {
                b.Property(o => o.Id).HasDefaultValueSql("nextval('\"Reminder\"')");
                b.Property<string>("ApplicationUserId");
                b.HasIndex("ApplicationUserId");
                b.ToTable("Reminders");
                b.HasOne(o => o.ApplicationUser).WithMany().HasForeignKey("ApplicationUserId");
            });
        }
    }
}
