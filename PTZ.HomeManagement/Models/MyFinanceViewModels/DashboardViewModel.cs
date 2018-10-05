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
            Balance = new DoughnutChartViewModel();
            MonthlyProgression = new LineChartViewModel();
            Categories = new BarChartViewModel();
        }

        public DoughnutChartViewModel Balance { get; set; }
        public LineChartViewModel MonthlyProgression { get; set; }
        public BarChartViewModel Categories { get; set; }
    }
}
