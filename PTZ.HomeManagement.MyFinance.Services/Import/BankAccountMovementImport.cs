using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.MyFinance.Models;

namespace PTZ.HomeManagement.MyFinance.Imports
{
    public abstract class BankAccountMovementImport
    {
        public DateTime ImportDate { get; set; }

        public abstract List<BankAccountMovement> GetBankAccountMovements(IFormFile file);
    }
}
