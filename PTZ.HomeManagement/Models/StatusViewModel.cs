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
        }

        public string MachineName
        {
            get
            {
                return System.Environment.MachineName;
            }
        }

        public double TotalMemory { get; internal set; }
        public string Environment { get; internal set; }
        public string DefaultConnection { get; internal set; }
        public bool AllowSignin { get; internal set; }
    }
}
