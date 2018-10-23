using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.ExpirationReminderModels
{
    public class ReminderListItemViewModel
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("ReminderType")]
        public ReminderType ReminderType { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("ExpirationDate")]
        public DateTime ExpirationDate { get; set; }

        [DisplayName("Categories")]
        public List<string> Categories { get; set; }
    }
}
