using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PTZ.HomeManagement.Models.MyFinance
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, MaxLength(3)]
        public string Abbreviation { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
