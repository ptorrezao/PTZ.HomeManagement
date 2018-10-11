using PTZ.HomeManagement.ExpirationReminder.Core.Resources;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.ExpirationReminder.Core.Enums
{
    public enum ReminderType
    {
        [LocalizedDescription("ReminderType_Document", typeof(ExpirationReminderResources))]
        Document,
        [LocalizedDescription("ReminderType_Service", typeof(ExpirationReminderResources))]
        Service
    }
}
