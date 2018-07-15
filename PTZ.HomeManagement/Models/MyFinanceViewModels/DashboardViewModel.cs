using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            DoughnutChart = new DoughnutChartViewModel();
            BarChart = new BarChartViewModel();
        }

        public DoughnutChartViewModel DoughnutChart { get; set; }
        public BarChartViewModel BarChart { get; set; }
    }
}
