namespace PTZ.HomeManagement.MyFinance.Imports
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
