using System.Collections.Generic;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class LineChartViewModel
    {
        public LineChartViewModel()
        {
            Movements = new List<DashboardMovementViewModel>();
        }

        public List<DashboardMovementViewModel> Movements { get;  set; }
    }
}