using Microsoft.AspNetCore.Mvc.Rendering;
using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.ExpirationReminderModels
{
    public class ReminderViewModel
    {
        public ReminderViewModel()
        {
            AvailableReminderTypes = Enum.GetValues(typeof(ReminderType)).Cast<ReminderType>().Select(x => { return new SelectListItem() { Value = x.ToString(), Text = EnumExtensions.GetDescription(x) }; });
        }

        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("ReminderType")]
        public ReminderType ReminderType { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("ExpirationDate")]
        public DateTime ExpirationDate { get; set; }

        public IEnumerable<SelectListItem> AvailableReminderTypes { get; private set; }
    }
}
