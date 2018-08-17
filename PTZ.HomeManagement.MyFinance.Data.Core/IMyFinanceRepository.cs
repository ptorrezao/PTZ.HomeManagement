using PTZ.HomeManagement.MyFinance.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.MyFinance.Data
{
    public interface IMyFinanceRepository
    {
        BankAccount GetBankAccount(string userId, long bankAccountId);
        List<BankAccount> GetBankAccounts(string userId);
        void SaveBankAccount(string userId, BankAccount bankAccount);
        void CommitChanges();
        void DeleteBankAccount(string userId, BankAccount bankAccount);
        BankAccountMovement GetBankAccountMovement(string userId, long bankAccountId, int movementId);
        List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified);
        List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, DateTime startDate, DateTime endDate);
        bool ExistsBankAccount(long bankAccountId, string userId);
        void SaveBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement);
        void SaveBankAccountMovements(string userId, long bankAccountId, List<BankAccountMovement> list);
        void DeleteBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement);
        bool ExistsBankAccountMovements(BankAccountMovement item);
        List<Category> GetCategories(string userId);
        Category GetCategory(string userId, long id);
        void SaveCategory(string userId, Category category);
        void DeleteCategory(string userId, Category category);
        void SetCategoriesToBankAccountMovement(string userId, long bankAccountMovementId, List<long> categories);
    }
}
