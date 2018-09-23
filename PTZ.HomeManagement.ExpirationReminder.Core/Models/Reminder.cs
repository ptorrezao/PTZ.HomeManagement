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
        public ApplicationUser ApplicationUser { get; set; }
    }
}
