using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.ExpirationReminder.Core
{
    public class Reminder
    {
        public long Id { get; set; }
        public ReminderType ReminderType { get; set; }
        public string Title { get; set; }
        public DateTime ExpirationDate { get; set; }

        public long NotifyInPeriodAmout { get; set; }
        public ReminderNotifyPeriodType NotifyInPeriodType { get; set; }
        public ReminderNotifyType NotifyType { get; set; }

        public string Notes { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public virtual List<ReminderCategoryReminder> Categories { get; set; }
        public bool Sent { get; set; }
        public DateTime? SentOn { get; set; }
    }
}
