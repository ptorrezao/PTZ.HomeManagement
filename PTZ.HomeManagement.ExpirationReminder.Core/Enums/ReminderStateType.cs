using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PTZ.HomeManagement.ExpirationReminder.Core.Enums
{
    public enum ReminderStateType
    {
        [Description("Em dia")]
        NonExpired,
        [Description("A Expirar")]
        Expiring,
        [Description("Expirado")]
        Expired
    }
}
