using PTZ.HomeManagement.MyFinance;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class DashboardAccountViewModel
    {
        public decimal Amount { get; internal set; }
        public AssetType AssetType { get; internal set; }
        public string AccountNumber { get; internal set; }
        public Bank Bank { get; internal set; }
        public string AccountTitle { get; internal set; }
    }
}