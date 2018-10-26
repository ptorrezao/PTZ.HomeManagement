using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PTZ.HomeManagement.ExpirationReminder.Core;
using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
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

        public void DeleteReminderCategory(string userId, ReminderCategory reminderCategory)
        {
            var elementsToRemove = this.context.Categories.Where(x => x.ApplicationUser.Id == userId && reminderCategory.Id == x.Id);
            this.context.Categories.RemoveRange(elementsToRemove);
        }

        public Reminder GetReminder(string userId, int id)
        {
            Reminder reminder = this.context.Reminders.Include(x => x.Categories).ThenInclude(x => x.Category).FirstOrDefault(x => x.Id == id && x.ApplicationUser.Id == userId);
            return reminder;
        }

        public ReminderCategory GetReminderCategory(string userId, int id)
        {
            ReminderCategory category = this.context.Categories.FirstOrDefault(x => x.Id == id && x.ApplicationUser.Id == userId);
            return category;
        }

        public List<Reminder> GetReminders(string userId)
        {
            List<Reminder> reminders = this.context.Reminders.Include(x => x.Categories).ThenInclude(x => x.Category).Where(x => x.ApplicationUser.Id == userId).ToList();
            return reminders;
        }

        public List<Reminder> GetRemindersByType(string userId, List<ReminderStateType> reminderStateType, bool? sentState = null)
        {
            List<Reminder> filteredReminders = new List<Reminder>();

            var reminders = this.context.Reminders.Include(x => x.Categories)
                                                    .ThenInclude(x => x.Category)
                                                    .Where(x => x.ApplicationUser.Id == userId);
            if (sentState != null)
            {
                reminders = reminders.Where(x => x.SentOn.HasValue == sentState);
            }

            if (!reminders.Any())
            {
                filteredReminders = reminders.ToList();
            }
            else
            {
                var currentDate = DateTime.Now;
                foreach (var reminderStateType in reminderStateTypes)
                {
                    switch (reminderStateType)
                    {
                        case ReminderStateType.NonExpired:
                            filteredReminders.AddRange(reminders.Where(x => x.ExpirationDate > currentDate && GetNotificationDate(x) >= currentDate));
                            break;
                        case ReminderStateType.Expiring:
                            filteredReminders.AddRange(reminders.Where(x => GetNotificationDate(x) <= currentDate && x.ExpirationDate >= currentDate));
                            break;
                        case ReminderStateType.Expired:
                            filteredReminders.AddRange(reminders.Where(x => x.ExpirationDate < currentDate && GetNotificationDate(x) < currentDate));
                            break;
                        default:
                            break;
                    }
                }
            }

            return filteredReminders;
        }

        private DateTime GetNotificationDate(Reminder reminder)
        {
            DateTime date = reminder.ExpirationDate;
            if (reminder.NotifyType != ReminderNotifyType.NoNotification)
            {
                switch (reminder.NotifyInPeriodType)
                {
                    case ReminderNotifyPeriodType.Days:
                        date = date.AddDays(-reminder.NotifyInPeriodAmout);
                        break;
                    case ReminderNotifyPeriodType.Months:
                        date = date.AddMonths(-Convert.ToInt32(reminder.NotifyInPeriodAmout));
                        break;
                    case ReminderNotifyPeriodType.Years:
                        date = date.AddYears(-Convert.ToInt32(reminder.NotifyInPeriodAmout));
                        break;
                    default:
                        break;
                }
            }
            return date;
        }

        public List<ReminderCategory> GetReminderCategories(string userId)
        {
            List<ReminderCategory> categories = this.context.Categories.Where(x => x.ApplicationUser.Id == userId).ToList();
            return categories;
        }

        public void SaveReminder(string userId, Reminder reminder)
        {
            this.context.Entry(reminder).State = reminder.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        public void SaveReminders(string userId, List<Reminder> reminders)
        {
            foreach (var reminder in reminders)
            {
                this.context.Entry(reminder).State = reminder.Id == 0 ? EntityState.Added : EntityState.Modified;
            }
        }

        public void SaveReminderCategory(string userId, ReminderCategory reminderCategory)
        {
            this.context.Entry(reminderCategory).State = reminderCategory.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        public void SetCategoriesToReminder(string userId, long id, List<long> selectedCategories)
        {
            selectedCategories = selectedCategories ?? new List<long>();
            if (context.Reminders.Any(x => x.Id == id))
            {
                var reminder = context.Reminders.First(x => x.Id == id);
                if (context.Reminders.Any(q => q.Id == id && !selectedCategories.Contains(q.Id)))
                {
                    var categoriesToRemove = context.CategoriesOnReminders.Where(q => q.ReminderId == id && !selectedCategories.Contains(q.CategoryId));
                    context.CategoriesOnReminders.RemoveRange(categoriesToRemove);
                }

                foreach (long categoryId in selectedCategories)
                {
                    if (categoryId > 0 &&
                        context.Categories.Any(x => x.Id == categoryId) &&
                        !context.CategoriesOnReminders.Any(x => x.CategoryId == categoryId && x.ReminderId == id))
                    {
                        context.CategoriesOnReminders.Add(new ReminderCategoryReminder()
                        {
                            Reminder = reminder,
                            ReminderId = reminder.Id,
                            CategoryId = categoryId,
                            Category = context.Categories.First(x => x.Id == categoryId),
                        });
                    }
                }
            }
            context.SaveChanges();
        }


    }
}
