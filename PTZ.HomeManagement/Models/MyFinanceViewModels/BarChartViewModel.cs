using System.Collections.Generic;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class BarChartViewModel
    {
        public BarChartViewModel()
        {
            Items = new List<BarChartItemViewModel>();
        }

        public List<BarChartItemViewModel> Items { get;  set; }
    }
}