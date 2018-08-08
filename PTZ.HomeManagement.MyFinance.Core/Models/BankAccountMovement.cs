using System;
using System.ComponentModel.DataAnnotations;

namespace PTZ.HomeManagement.MyFinance.Models
{
    public class BankAccountMovement
    {
        [Key]
        public int Id { get; set; }

        public DateTime MovementDate { get; set; }

        public DateTime ValueDate { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalBalanceAfterMovement { get; set; }

        public string Description { get; set; }

        public virtual BankAccount BankAccount { get; set; }

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
                ob.Amount == this.Amount &&
                ob.TotalBalanceAfterMovement == this.TotalBalanceAfterMovement &&
                ob.Description == this.Description;
        }
    }
}