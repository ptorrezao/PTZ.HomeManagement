using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.MyFinance.Models
{
    public class CategoryBankAccountMovement
    {
        public long BankAccountMovementId { get; set; }
        public BankAccountMovement BankAccountMovement { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
