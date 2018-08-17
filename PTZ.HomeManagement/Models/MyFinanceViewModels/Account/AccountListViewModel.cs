using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class AccountListViewModel
    {
        public AccountListViewModel()
        {
            Items = new List<AccountListItemViewModel>();
        }

        public List<AccountListItemViewModel> Items { get; set; }
    }
}
