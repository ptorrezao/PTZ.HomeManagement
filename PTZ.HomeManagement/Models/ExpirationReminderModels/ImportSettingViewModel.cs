using Microsoft.AspNetCore.Mvc.Rendering;
using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PTZ.HomeManagement.Models.ExpirationReminderModels
{
    public class ImportSettingViewModel
    {
        public ImportSettingViewModel()
        {
            AvailableTitleRexgexTargetTypes = Enum.GetValues(typeof(RegexTarget)).Cast<RegexTarget>().Select(x => { return new SelectListItem() { Value = x.ToString(), Text = EnumExtensions.GetDescription(x) }; });
            AvailableExpirationDateRexgexTargetTypes = Enum.GetValues(typeof(RegexTarget)).Cast<RegexTarget>().Select(x => { return new SelectListItem() { Value = x.ToString(), Text = EnumExtensions.GetDescription(x) }; });
        }

        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Nome")]
        public string Name { get; set; }

        [DisplayName("Titulo")]
        public string TitleFormat { get; set; }

        [DisplayName("Labels")]
        public string Label { get; set; }

        [DisplayName("Rexgex")]
        public string TitleRexgex { get; set; }

        [DisplayName("Alvo")]
        public RegexTarget TitleRexgexTarget { get; set; }

        [DisplayName("Rexgex")]
        public string ExpirationDateRexgex { get; set; }

        [DisplayName("Alvo")]
        public RegexTarget ExpirationDateRexgexTarget { get; set; }

        public IEnumerable<SelectListItem> AvailableTitleRexgexTargetTypes { get; private set; }

        public IEnumerable<SelectListItem> AvailableExpirationDateRexgexTargetTypes { get; private set; }
    }
}
