using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models
{
    public class StatusViewModel
    {
        public StatusViewModel()
        {
            LogEntries = new List<string>();
        }

        public string MachineName
        {
            get
            {
                return System.Environment.MachineName;
            }
        }

        public List<string> LogEntries { get; set; }
        public double TotalMemory { get; internal set; }
        public string Environment { get; internal set; }
    }
}
