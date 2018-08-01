using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PTZ.HomeManagement.Models.MyFinance
{
    public class BankAccountMovement
    {
        [Key]
        public int Id { get; set; }

        public DateTime MovementDate { get; set; }

        public DateTime ValueDate { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalBalanceAfterMovement{ get; set; }

        public string Description { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}
