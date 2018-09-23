using System;
using System.Collections.Generic;
using PTZ.HomeManagement.ExpirationReminder.Core;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core
{
    public interface IExpirationReminderRepository
    {
        void DeleteReminder(string userId, Reminder reminder);
        void CommitChanges();
        Reminder GetReminder(string userId, int id);
        List<Reminder> GetReminders(string userId);
        void SaveReminder(string userId, Reminder reminder);
    }
}
