using System;
using System.Collections.Generic;
using System.Text;
using PTZ.HomeManagement.Models.MyFinance;

namespace PTZ.HomeManagement.Services
{
    public interface IMyFinanceService
    {
        BankAccount GetBankAccountDefault(string userId);
        BankAccount GetBankAccount(string userId, int bankAccountId);
        List<BankAccount> GetBankAccounts(string userId);
        void SaveBankAccount(string userId, BankAccount bankAccount);
        void DeleteBankAccount(string userId, BankAccount bankAccount);
    }
}
