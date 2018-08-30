using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTZ.HomeManagement.MyFinance.Models
{
    public class BankAccountMovement
    {
        public long Id { get; set; }

        public DateTime MovementDate { get; set; }

        public DateTime ValueDate { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalBalanceAfterMovement { get; set; }

        public string Description { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        public virtual List<CategoryBankAccountMovement> Categories { get; set; }

        public override int GetHashCode()
        {
            return MovementDate.GetHashCode() +
                ValueDate.GetHashCode() +
                Amount.GetHashCode() +
                TotalBalanceAfterMovement.GetHashCode() +
                Description.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            BankAccountMovement ob = obj as BankAccountMovement;

            return ob.MovementDate == this.MovementDate &&
                ob.ValueDate == this.ValueDate &&
                ob.Id == this.Id &&
                ob.Amount == this.Amount &&
                (ob.BankAccount != null ? ob.BankAccount.Id : 0) == (this.BankAccount != null ? this.BankAccount.Id : 0) &&
                ob.TotalBalanceAfterMovement == this.TotalBalanceAfterMovement &&
                ob.Description == this.Description;
        }
    }
}