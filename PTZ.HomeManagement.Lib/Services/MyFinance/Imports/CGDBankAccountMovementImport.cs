using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.Models.MyFinance;

namespace PTZ.HomeManagement.Services.MyFinance
{
    public abstract class BankAccountMovementImport
    {
        public DateTime ImportDate { get; set; }

        public abstract List<BankAccountMovement> GetBankAccountMovements(IFormFile file);
    }
}
