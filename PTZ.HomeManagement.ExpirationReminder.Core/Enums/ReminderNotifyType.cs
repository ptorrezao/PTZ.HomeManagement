using PTZ.HomeManagement.ExpirationReminder.Core.Resources;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.ExpirationReminder.Core.Enums
{
    public enum ReminderNotifyType
    {
        [LocalizedDescription("ReminderNotifyType_NoNotification", typeof(ExpirationReminderResources))]
        NoNotification,

        [LocalizedDescription("ReminderNotifyType_Email", typeof(ExpirationReminderResources))]
        Email
    }
}
