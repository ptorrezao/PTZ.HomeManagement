using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PTZ.HomeManagement.ExpirationReminder.Core;
using PTZ.HomeManagement.ExpirationReminder.Data.Core.EF;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core
{
    public class ExpirationReminderRepository : IExpirationReminderRepository
    {
        private readonly ExpirationReminderDbContext context;

        public ExpirationReminderRepository(IServiceProvider serviceProvider)
        {
            try
            {
                DbContextOptions<ExpirationReminderDbContext> options = serviceProvider.GetRequiredService<DbContextOptions<ExpirationReminderDbContext>>();
                this.context = new ExpirationReminderDbContext(options);

                if (!options.Extensions.Any(x => x.GetType() == typeof(InMemoryOptionsExtension)) &&
                    (!this.context.Database.GetAppliedMigrations().Any(x => x == this.context.Database.GetMigrations().LastOrDefault())))
                {
                    this.context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ExpirationReminderRepository>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        public void CommitChanges()
        {
            this.context.SaveChanges();
        }

        public void DeleteReminder(string userId, Reminder reminder)
        {
            var elementsToRemove = this.context.Reminders.Where(x => x.ApplicationUser.Id == userId && reminder.Id == x.Id);
            this.context.Reminders.RemoveRange(elementsToRemove);
        }

        public Reminder GetReminder(string userId, int id)
        {
            Reminder reminder = this.context.Reminders.FirstOrDefault(x => x.Id == id && x.ApplicationUser.Id == userId);
            return reminder;
        }

        public List<Reminder> GetReminders(string userId)
        {
            List<Reminder> reminders = this.context.Reminders.Where(x =>  x.ApplicationUser.Id == userId).ToList();
            return reminders;
        }

        public void SaveReminder(string userId, Reminder reminder)
        {
            this.context.Entry(reminder).State = reminder.Id == 0 ? EntityState.Added : EntityState.Modified;
        }
    }
}
