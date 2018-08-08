using PTZ.HomeManagement.MyFinance.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.MyFinance.Data
{
    public interface IMyFinanceRepository
    {
        BankAccount GetBankAccount(string userId, int bankAccountId);
        List<BankAccount> GetBankAccounts(string userId);
        void SaveBankAccount(string userId, BankAccount bankAccount);
        void CommitChanges();
        void DeleteBankAccount(string userId, BankAccount bankAccount);
        BankAccountMovement GetBankAccountMovement(string userId, int bankAccountId, int movementId);
        List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified);
        List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, DateTime startDate, DateTime endDate);
        bool ExistsBankAccount(int bankAccountId, string userId);
        void SaveBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement);
        void SaveBankAccountMovements(string userId, int bankAccountId, List<BankAccountMovement> list);
        void DeleteBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement);
        bool ExistsBankAccountMovements(BankAccountMovement item);
    }
}
