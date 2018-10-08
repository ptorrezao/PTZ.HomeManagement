using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.ExpirationReminder.Core
{
    public class ReminderCategoryReminder
    {
        public long ReminderId { get; set; }
        public Reminder Reminder { get; set; }
        public long CategoryId { get; set; }
        public ReminderCategory Category { get; set; }
    }
}
