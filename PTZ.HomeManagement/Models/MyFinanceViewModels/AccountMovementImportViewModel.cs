using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.Utils;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class AccountMovementImportViewModel
    {
        public AccountMovementImportViewModel()
        {
            AvailableImportTypes = Enum.GetValues(typeof(BankAccountMovementImportType)).Cast<BankAccountMovementImportType>().Select(x => { return new SelectListItem() { Value = x.ToString(), Text = EnumExtensions.GetDescription(x) }; });
        }

        public AccountMovementImportViewModel(long bankAccountId) : this()
        {
            BankAccountId = BankAccountId == 0 ? bankAccountId : BankAccountId;
        }

        public long BankAccountId { get; set; }
        public BankAccountMovementImportType ImportType { get; set; }
        public IEnumerable<SelectListItem> AvailableImportTypes { get; set; }
        public List<AccountMovementReviewListItemViewModel> Items { get; set; }
    }
}
