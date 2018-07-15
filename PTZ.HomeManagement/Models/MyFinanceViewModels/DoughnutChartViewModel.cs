using System.Collections.Generic;
using System.Linq;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class DoughnutChartViewModel
    {
        public DoughnutChartViewModel()
        {
            Assets = new List<DashboardAccountViewModel>();
            Liabilities = new List<DashboardAccountViewModel>();
        }


        public List<DashboardAccountViewModel> Assets { get; set; }

        public decimal AssetsAccountingBalancing
        {
            get
            {
                return this.Assets.Sum(x => x.Amount);
            }
        }

        public List<DashboardAccountViewModel> Liabilities { get; set; }

        public decimal LiabilitiesAccountingBalancing
        {
            get
            {
                return this.Liabilities.Sum(x => x.Amount);
            }
        }
    }
}
