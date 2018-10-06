using PTZ.HomeManagement.MyFinance;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class DoughnutChartItemViewModel
    {
        public decimal Amount { get;  set; }
        public string Color { get;  set; }
        public string Group { get;  set; }
        public string XAxis { get; set; }
        public string AccountTitle { get; set; }
        public string AccountNumber { get; set; }
        public string AssetType { get; set; }
    }
}