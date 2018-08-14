using System.ComponentModel;

namespace PTZ.HomeManagement.MyFinance
{
    public enum BankAccountMovementImportType
    {
        [Description("Caixa Geral de Depósitos")]
        CGD = 0,
        [Description("Banco Português de Investimento")]
        BPI = 1
    }
}
