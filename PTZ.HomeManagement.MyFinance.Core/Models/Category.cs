using System;
using System.Collections.Generic;
using System.Text;
using PTZ.HomeManagement.Models;

namespace PTZ.HomeManagement.MyFinance.Models
{
    public class Category
    {
        public ApplicationUser ApplicationUser { get; set; }
        public virtual List<CategoryBankAccountMovement> Movements { get; set; }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
}
