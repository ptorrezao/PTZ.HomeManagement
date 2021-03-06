﻿using PTZ.HomeManagement.ExpirationReminder.Core;
using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
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
        List<Reminder> GetRemindersByType(string userId, List<ReminderStateType> reminderStateTypes);
        ReminderCategory GetReminderCategory(string userId, int id);
        ReminderCategory GetReminderCategoryDefault(string userId);
        void SaveReminderCategory(string userId, ReminderCategory reminderCategory);
        void DeleteReminderCategory(string userId, ReminderCategory reminderCategory);
        void SetCategoriesToReminder(string userId, long id, List<long> selectedCategories);

        List<ImportSetting> GetImportSettings(string userId);
        ImportSetting GetImportSettingDefault(string userId);
        ImportSetting GetImportSetting(string userId, int id);
        void SaveImportSetting(string userId, ImportSetting importSetting);
        void DeleteImportSetting(string userId, ImportSetting importSetting);
    }
}
