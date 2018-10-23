using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.ExpirationReminder.Core
{
    public class ReminderCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public virtual List<ReminderCategoryReminder> Reminders { get; set; }
    }
}
