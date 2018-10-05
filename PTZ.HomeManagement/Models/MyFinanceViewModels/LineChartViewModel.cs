using System.Collections.Generic;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class LineChartViewModel
    {
        public LineChartViewModel()
        {
            Movements = new List<LineChartItemViewModel>();
        }

        public List<LineChartItemViewModel> Movements { get;  set; }
    }
}