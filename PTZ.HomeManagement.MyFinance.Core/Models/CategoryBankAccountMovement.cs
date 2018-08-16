using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.MyFinance.Models
{
    public class CategoryBankAccountMovement
    {
        public int BankAccountMovementId { get; set; }
        public BankAccountMovement BankAccountMovement { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
