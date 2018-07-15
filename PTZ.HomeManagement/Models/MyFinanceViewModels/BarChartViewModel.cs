using System.Collections.Generic;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class BarChartViewModel
    {
        public BarChartViewModel()
        {
            Movements = new List<DashboardMovementViewModel>();
        }

        public List<DashboardMovementViewModel> Movements { get;  set; }
    }
}