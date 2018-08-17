using PTZ.HomeManagement.MyFinance;
using System.ComponentModel;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class AccountListItemViewModel
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Bank")]
        public Bank Bank { get; set; }

        [DisplayName("Color")]
        public string Color { get; set; }
        
        [DisplayName("IBAN")]
        public string IBAN { get; set; }

        [DisplayName("AccountType")]
        public AssetType AccountType { get; set; }
    }
}