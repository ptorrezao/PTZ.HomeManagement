using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.ExpirationReminderModels
{
    public class ReminderOverviewListItem
    {
        public ReminderStateType Type { get; set; }
        public int Quantity { get; set; }
    }
}
