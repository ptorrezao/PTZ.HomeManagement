using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MailKit;
using MailKit.Net.Imap;
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
            list.Add(this.SendEmailsForExpiredAndExpiringReminders);
            list.Add(this.GetRemindersFromEmails);

            return list;
        }

        public List<ImportSetting> GetImportSettings(string userId)
        {
            return expirationRepo.GetImportSettings(userId);
        }

        public ImportSetting GetImportSetting(string userId, int id)
        {
            return expirationRepo.GetImportSetting(userId, id);
        }

        public void SaveImportSetting(string userId, ImportSetting importSetting)
        {
            importSetting.ApplicationUser = appRepo.GetUser(userId);
            expirationRepo.SaveImportSetting(userId, importSetting);
            expirationRepo.CommitChanges();
        }

        public void DeleteImportSetting(string userId, ImportSetting importSetting)
        {
            expirationRepo.DeleteImportSetting(userId, importSetting);
            expirationRepo.CommitChanges();
        }

        private string GetRemindersFromEmails()
        {
            var users = appRepo.GetUsers();

            foreach (var user in users)
            {
                if (user.HasGmailAppPassword)
                {
                    List<ImportSetting> importSettings = this.GetImportSettings(user.Id);

                    using (var client = new ImapClient())
                    {
                        // For demo-purposes, accept all SSL certificates
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.Connect("imap.gmail.com", 993, true);
                        client.Authenticate(user.Email, user.AppPassword);

                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadWrite);
                        var existingReminders = this.GetReminders(user.Id);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var messageSummary = inbox.Fetch(new List<int>() { i }, MessageSummaryItems.GMailLabels).First();

                            foreach (var importSetting in importSettings)
                            {
                                if (messageSummary.GMailLabels.Any(x => x == importSetting.Label))
                                {
                                    var message = inbox.GetMessage(i);

                                    string reminderTitle = ExtractRegex(importSetting.TitleRexgex, importSetting.TitleRexgexTarget, message);
                                    string expirationDateString = ExtractRegex(importSetting.ExpirationDateRexgex, importSetting.ExpirationDateRexgexTarget, message);

                                    if (!string.IsNullOrEmpty(reminderTitle) &&
                                        !string.IsNullOrEmpty(expirationDateString))
                                    {
                                        DateTime expirationDate;
                                        DateTime.TryParse(expirationDateString, out expirationDate);

                                        Reminder reminder = new Reminder()
                                        {
                                            Title = string.Format(importSetting.TitleFormat, reminderTitle),
                                            Notes = message.HtmlBody ?? message.TextBody,
                                            ExpirationDate = expirationDate,
                                            NotifyInPeriodAmout = 5,
                                            NotifyInPeriodType = ReminderNotifyPeriodType.Days,
                                            NotifyType = ReminderNotifyType.Email,
                                            ReminderType = ReminderType.Service,
                                        };

                                        if (!existingReminders.Any(x => x.Title == reminder.Title))
                                        {
                                            this.SaveReminder(user.Id, reminder);
                                        }
                                    }

                                }
                            }

                        }

                        client.Disconnect(true);
                    }
                }
            }

            return "";
        }



        private string ExtractRegex(string rexgex, RegexTarget regexTarget, MimeKit.MimeMessage message)
        {
            Regex regex = new Regex(rexgex);
            Match match = null;
            switch (regexTarget)
            {
                case RegexTarget.Body:
                    match = regex.Match(message.HtmlBody ?? message.TextBody);
                    break;
                case RegexTarget.Subject:
                    match = regex.Match(message.Subject);
                    break;
                case RegexTarget.Sender:
                    match = regex.Match(message.Sender.Name);
                    break;
                default:
                    break;
            }
            return match != null && match.Success ? match.Value : "";
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
                    reminderStateTypes: new List<ReminderStateType>() { ReminderStateType.Expired, ReminderStateType.Expiring },
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
                        messagesSent = UpdateReminder(messagesSent, user, reminders);
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
            return PrepareMessage(messagesSent, result);
        }

        private string PrepareMessage(int messagesSent, StringBuilder result)
        {
            if (result.Length <= 0 && messagesSent > 0)
            {
                result.AppendLine(string.Format("{0} emails were sent.", messagesSent));
            }

            return result.ToString();
        }

        private int UpdateReminder(int messagesSent, ApplicationUser user, List<Reminder> reminders)
        {
            messagesSent += 1;
            foreach (var reminder in reminders)
            {
                reminder.Sent = true;
                reminder.SentOn = DateTime.Now;
            }

            expirationRepo.SaveReminders(user.Id, reminders);
            expirationRepo.CommitChanges();
            return messagesSent;
        }

        public string GetName()
        {
            return this.GetType().Name;
        }

        public List<Reminder> GetRemindersByType(string userId, List<ReminderStateType> reminderStateTypes)
        {
            return expirationRepo.GetRemindersByType(userId, reminderStateTypes);
        }

        public ImportSetting GetImportSettingDefault(string userId)
        {
            ApplicationUser user = appRepo.GetUser(userId);
            return new ImportSetting()
            {
                ApplicationUser = user
            };
        }
    }
}
