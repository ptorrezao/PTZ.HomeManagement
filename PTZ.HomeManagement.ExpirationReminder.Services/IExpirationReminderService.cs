using PTZ.HomeManagement.ExpirationReminder.Core;
using PTZ.HomeManagement.Interfaces;
using System.Collections.Generic;

namespace PTZ.HomeManagement.ExpirationReminder.Services
{
    public interface IExpirationReminderService : IWorker
    { 
        List<Reminder> GetReminders(string userId);
        Reminder GetReminder(string userId, int id);
        Reminder GetReminderDefault(string userId);
        void SaveReminder(string userId, Reminder reminder);
        void DeleteReminder(string userId, Reminder reminder);

        List<ReminderCategory> GetReminderCategories(string userId);
        ReminderCategory GetReminderCategory(string userId, int id);
        ReminderCategory GetReminderCategoryDefault(string userId);
        void SaveReminderCategory(string userId, ReminderCategory reminderCategory);
        void DeleteReminderCategory(string userId, ReminderCategory reminderCategory);
        void SetCategoriesToReminder(string userId, long id, List<long> selectedCategories);
    }
}
