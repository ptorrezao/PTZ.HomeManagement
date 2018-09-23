using System;
using System.Collections.Generic;
using System.Text;
using PTZ.HomeManagement.Core.Data;
using PTZ.HomeManagement.ExpirationReminder.Core;
using PTZ.HomeManagement.ExpirationReminder.Data.Core;
using PTZ.HomeManagement.Models;

namespace PTZ.HomeManagement.ExpirationReminder.Services
{
    public class ExpirationReminderService : IExpirationReminderService
    {
        private readonly IExpirationReminderRepository expirationRepo;
        private readonly IApplicationRepository appRepo;

        public ExpirationReminderService(IExpirationReminderRepository repo,
            IApplicationRepository dbContext)
        {
            this.expirationRepo = repo;
            this.appRepo = dbContext;
        }

        public void DeleteReminder(string userId, Reminder reminder)
        {
            expirationRepo.DeleteReminder(userId, reminder);
            expirationRepo.CommitChanges();
        }

        public Reminder GetReminder(string userId, int id)
        {
            return expirationRepo.GetReminder(userId, id);
        }

        public Reminder GetReminderDefault(string userId)
        {
            ApplicationUser user = appRepo.GetUser(userId);
            return new Reminder()
            {
                ApplicationUser = user,
                ExpirationDate = DateTime.Now,
            };
        }

        public List<Reminder> GetReminders(string userId)
        {
            return expirationRepo.GetReminders(userId);
        }

        public void SaveReminder(string userId, Reminder reminder)
        {
            reminder.ApplicationUser = appRepo.GetUser(userId);
            expirationRepo.SaveReminder(userId, reminder);
            expirationRepo.CommitChanges();
        }
    }
}
