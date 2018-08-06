using PTZ.HomeManagement.Models.MyFinance;
using System.ComponentModel;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class AccountListItemViewModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Bank")]
        public Bank Bank { get; set; }

        [DisplayName("IBAN")]
        public string IBAN { get; set; }

        [DisplayName("AccountType")]
        public AssetType AccountType { get; set; }
    }
}