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

        public void SetAvailableCategories(List<ReminderCategoryViewModel> value)
        {
            this.AvailableCategories = new List<SelectListItem>();
            this.SelectedCategories = this.SelectedCategories ?? new List<long>();
            foreach (var item in value)
            {
                AvailableCategories.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Name, Selected = this.SelectedCategories.Contains(item.Id) });
            }
        }

        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("ReminderType")]
        public ReminderType ReminderType { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("ExpirationDate")]
        public DateTime ExpirationDate { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        [DisplayName("NotifyInPeriodAmout")]
        public long NotifyInPeriodAmout { get; set; }

        [DisplayName("NotifyInPeriodType")]
        public ReminderNotifyPeriodType NotifyInPeriodType { get; set; }

        [DisplayName("NotifyType")]
        public ReminderNotifyType NotifyType { get; set; }

        [DisplayName("SelectedCategories")]
        public List<long> SelectedCategories { get; set; }

        public List<SelectListItem> AvailableCategories { get; set; }

        public IEnumerable<SelectListItem> AvailableReminderTypes { get; private set; }
    }
}
