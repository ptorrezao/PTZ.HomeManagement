using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class AccountMovementListViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<AccountMovementListItemViewModel> Items { get; set; }
    }
}
