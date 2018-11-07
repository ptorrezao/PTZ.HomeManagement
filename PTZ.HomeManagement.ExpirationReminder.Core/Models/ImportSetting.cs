using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.ExpirationReminder.Core
{
    public class ImportSetting
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Label { get; set; }
        public string TitleRexgex { get; set; }
        public RegexTarget TitleRexgexTarget { get; set; }
        public string TitleFormat { get; set; }
        public string ExpirationDateRexgex { get; set; }
        public RegexTarget ExpirationDateRexgexTarget { get; set; }
    }
}
