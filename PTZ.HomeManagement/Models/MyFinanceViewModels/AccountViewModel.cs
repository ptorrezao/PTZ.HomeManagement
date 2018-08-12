using Microsoft.AspNetCore.Mvc.Rendering;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class AccountViewModel
    {
        public AccountViewModel()
        {
            AvailableBanks = Enum.GetValues(typeof(Bank)).Cast<Bank>().Select(x => { return new SelectListItem() { Value = x.ToString(), Text = EnumExtensions.GetDescription(x) }; });
            AvailableAccountType = Enum.GetValues(typeof(AssetType)).Cast<AssetType>().Select(x => { return new SelectListItem() { Value = x.ToString(), Text = EnumExtensions.GetDescription(x) }; });
        }

        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("IBAN")]
        public string IBAN { get; set; }

        [DisplayName("AccountType")]
        public AssetType AccountType { get; set; }

        [DisplayName("Bank")]
        public Bank Bank { get; set; }

        public IEnumerable<SelectListItem> AvailableBanks { get; set; }
        public IEnumerable<SelectListItem> AvailableAccountType { get; set; }
    }
}
