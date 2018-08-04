using PTZ.HomeManagement.Migrations;
using PTZ.HomeManagement.Models.MyFinance;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.Services.MyFinance
{
    public class BankAccountMovementImportFactory
    {
        public BankAccountMovementImport GetBankAccountMovementImport(BankAccountMovementImportType importType)
        {
            switch (importType)
            {
                case BankAccountMovementImportType.CGD:
                    return new BankAccountMovementImportCgd();
                default:
                    return null;
            }
        }
    }
}
