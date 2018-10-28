using System.Collections.Generic;
using System.Linq;
using PTZ.HomeManagement.Components.MyFinance;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class DoughnutChartViewModel
    {
        public DoughnutChartViewModel()
        {
            Assets = new List<DoughnutChartItemViewModel>();
        }

        public List<DoughnutChartItemViewModel> Assets { get; set; }
    }
}
