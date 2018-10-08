using Microsoft.EntityFrameworkCore;
using PTZ.HomeManagement.ExpirationReminder.Core;
using System;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core.EF
{
    public class ExpirationReminderDbContext : DbContext
    {
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<ReminderCategory> Categories { get; set; }
        public DbSet<ReminderCategoryReminder> CategoriesOnReminders { get; set; }

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

            modelBuilder.HasSequence<long>("ReminderCategory").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<ReminderCategory>(b =>
            {
                b.Property(o => o.Id).HasDefaultValueSql("nextval('\"ReminderCategory\"')");
                b.Property<string>("ApplicationUserId");
                b.HasIndex("ApplicationUserId");
                b.ToTable("ReminderCategories");
                b.HasOne(o => o.ApplicationUser).WithMany().HasForeignKey("ApplicationUserId");
            });

            modelBuilder.Entity<ReminderCategoryReminder>(b =>
            {
                b.HasKey(q => new { q.ReminderId, q.CategoryId });
                b.HasOne(q => q.Category).WithMany(q => q.Reminders).HasForeignKey(x => x.CategoryId);
                b.HasOne(q => q.Reminder).WithMany(q => q.Categories).HasForeignKey(x => x.ReminderId);
            });
        }
    }
}
