using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTZ.HomeManagement.Core.Data;
using PTZ.HomeManagement.ExpirationReminder.Core;
using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using PTZ.HomeManagement.ExpirationReminder.Data.Core;
using PTZ.HomeManagement.Interfaces;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Services;

namespace PTZ.HomeManagement.ExpirationReminder.Services
{
    public class ExpirationReminderService : IExpirationReminderService
    {
        private readonly IExpirationReminderRepository expirationRepo;
        private readonly IApplicationRepository appRepo;
        private readonly IEmailSender emailSender;


        public ExpirationReminderService(IExpirationReminderRepository repo,
            IApplicationRepository dbContext,
            IEmailSender emailSenderService)
        {
            this.expirationRepo = repo;
            this.appRepo = dbContext;
            this.emailSender = emailSenderService;
        }

        public void DeleteReminder(string userId, Reminder reminder)
        {
            expirationRepo.DeleteReminder(userId, reminder);
            expirationRepo.CommitChanges();
        }

        public void DeleteReminderCategory(string userId, ReminderCategory reminderCategory)
        {
            expirationRepo.DeleteReminderCategory(userId, reminderCategory);
            expirationRepo.CommitChanges();
        }

        public Reminder GetReminder(string userId, int id)
        {
            return expirationRepo.GetReminder(userId, id);
        }

        public ReminderCategory GetReminderCategory(string userId, int id)
        {
            return expirationRepo.GetReminderCategory(userId, id);
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

        public ReminderCategory GetReminderCategoryDefault(string userId)
        {
            ApplicationUser user = appRepo.GetUser(userId);
            return new ReminderCategory()
            {
                ApplicationUser = user,
            };
        }

        public List<Reminder> GetReminders(string userId)
        {
            return expirationRepo.GetReminders(userId);
        }

        public List<ReminderCategory> GetReminderCategories(string userId)
        {
            return expirationRepo.GetReminderCategories(userId);
        }

        public void SaveReminder(string userId, Reminder reminder)
        {
            reminder.ApplicationUser = appRepo.GetUser(userId);
            expirationRepo.SaveReminder(userId, reminder);
            expirationRepo.CommitChanges();
        }

        public void SaveReminderCategory(string userId, ReminderCategory reminderCategory)
        {
            reminderCategory.ApplicationUser = appRepo.GetUser(userId);
            expirationRepo.SaveReminderCategory(userId, reminderCategory);
            expirationRepo.CommitChanges();
        }

        public void SetCategoriesToReminder(string userId, long id, List<long> selectedCategories)
        {
            expirationRepo.SetCategoriesToReminder(userId, id, selectedCategories);
        }

        public List<IWorkerJob> GetJobs()
        {
            List<IWorkerJob> list = new List<IWorkerJob>();
            list.Add(SendEmailsForExpiredAndExpiringReminders);

            return list;
        }

        public string SendEmailsForExpiredAndExpiringReminders()
        {
            var messagesSent = 0;
            var users = appRepo.GetUsers();
            var result = new StringBuilder("");

            foreach (var user in users)
            {
                var reminders = expirationRepo.GetRemindersByType(
                    userId: user.Id,
                    reminderStateType: new List<ReminderStateType>() { ReminderStateType.Expired, ReminderStateType.Expiring },
                    sentState: false);

                if (reminders.Any())
                {
                    var email = emailSender.SendEmail(
                           template: "SendEmailsForExpiredAndExpiringReminders",
                           subject: "PTZ.HomeAssistant - Reminders - " + DateTime.Now.ToShortDateString(),
                           toEmail: user.Email,
                           model: new
                           {
                               Email = user.Email,
                               Reminders = reminders,
                               Subject = "PTZ.HomeAssistant - Reminder - " + DateTime.Now.ToShortDateString(),
                               Link = "https://ptorrezao.pw"
                           },
                           path: @"/EmailTemplates/");

                    if (email.Successful)
                    {
                        messagesSent += 1;
                        foreach (var reminder in reminders)
                        {
                            reminder.Sent = true;
                            reminder.SentOn = DateTime.Now;
                        }

                        expirationRepo.SaveReminders(user.Id, reminders);
                        expirationRepo.CommitChanges();
                    }
                    else
                    {
                        foreach (var item in email.ErrorMessages)
                        {
                            result.AppendLine(item);
                        }
                    }
                }
            }

            if (result.Length <= 0 && messagesSent > 0)
            {
                result.AppendLine(string.Format("{0} emails were sent.", messagesSent));
            }

            return result.ToString();
        }

        public string GetName()
        {
            return this.GetType().Name;
        }
    }
}
