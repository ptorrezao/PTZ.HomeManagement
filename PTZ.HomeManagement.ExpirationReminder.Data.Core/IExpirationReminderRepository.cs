using System;
using System.Collections.Generic;
using PTZ.HomeManagement.ExpirationReminder.Core;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core
{
    public interface IExpirationReminderRepository
    {
        void CommitChanges();

        Reminder GetReminder(string userId, int id);
        ReminderCategory GetReminderCategory(string userId, int id);

        List<Reminder> GetReminders(string userId);
        List<ReminderCategory> GetReminderCategories(string userId);

        void SaveReminder(string userId, Reminder reminder);
        void SaveReminderCategory(string userId, ReminderCategory reminderCategory);

        void DeleteReminder(string userId, Reminder reminder);
        void DeleteReminderCategory(string userId, ReminderCategory reminderCategory);
        void SetCategoriesToReminder(string userId, long id, List<long> selectedCategories);
    }
}
