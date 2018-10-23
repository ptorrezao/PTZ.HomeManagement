using PTZ.HomeManagement.ExpirationReminder.Core.Resources;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.ExpirationReminder.Core.Enums
{
    public enum ReminderNotifyPeriodType
    {
        [LocalizedDescription("ReminderNotifyPeriodType_Days", typeof(ExpirationReminderResources))]
        Days,
        [LocalizedDescription("ReminderNotifyPeriodType_Months", typeof(ExpirationReminderResources))]
        Months,
        [LocalizedDescription("ReminderNotifyPeriodType_Years", typeof(ExpirationReminderResources))]
        Years
    }
}
