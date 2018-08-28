using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PTZ.HomeManagement.MyFinance.Models
{
    public class BankAccount
    {
        public BankAccount()
        {
            IsVisible = true;
            Movements = new List<BankAccountMovement>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual List<BankAccountMovement> Movements { get; set; }

        public bool IsVisible { get; set; }

        public AssetType AccountType { get; set; }

        public string IBAN { get; set; }

        public Bank Bank { get; set; }

        [NotMapped]
        public decimal CurrentBalance { get; set; }

        public string Color { get; set; }
    }
}
