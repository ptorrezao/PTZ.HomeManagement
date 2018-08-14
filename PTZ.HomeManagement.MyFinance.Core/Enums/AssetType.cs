using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PTZ.HomeManagement.MyFinance
{
    public enum AssetType
    {
        [Description("Conta Corrente")]
        CurrentAccount = 0,
        [Description("Conta Depósito")]
        DepositsAccount = 1,
        [Description("Conta Reforma")]
        RetirementSavingsAccount = 2,
        [Description("Fundos")]
        FundsAccount = 3,
        [Description("Conta Poupança")]
        SavingsAccount = 4
    }
}
