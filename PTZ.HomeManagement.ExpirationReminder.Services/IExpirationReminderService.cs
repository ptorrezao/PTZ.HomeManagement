using PTZ.HomeManagement.ExpirationReminder.Core;
using System.Collections.Generic;

namespace PTZ.HomeManagement.ExpirationReminder.Services
{
    public interface IExpirationReminderService
    {
        List<Reminder> GetReminders(string userId);
        Reminder GetReminder(string userId, int id);
        Reminder GetReminderDefault(string userId);
        void SaveReminder(string userId, Reminder reminder);
        void DeleteReminder(string userId, Reminder reminder);
    }
}
