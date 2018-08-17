using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.MyFinance.Models;

namespace PTZ.HomeManagement.MyFinance
{
    public interface IMyFinanceService
    {
        BankAccount GetBankAccountDefault(string userId);
        BankAccountMovement GetBankAccountMovementDefault(string userId, long bankAccountId);

        BankAccount GetBankAccount(string userId, long bankAccountId);
        List<BankAccount> GetBankAccounts(string userId);
        void SaveBankAccount(string userId, BankAccount bankAccount);
        void DeleteBankAccount(string userId, BankAccount bankAccount);

        BankAccountMovement GetBankAccountMovement(string userId, long bankAccountId, int movementId);
        List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, DateTime startDate, DateTime endDate);
        List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified);
        void SaveBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement);
        List<BankAccountMovement> SaveBankAccountMovements(string userId, long bankAccountId, List<BankAccountMovement> list);
        void DeleteBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement);
        void SetCategoriesToBankAccountMovement(string userId, long bankAccountMovementId, List<long> categories);

        List<BankAccountMovement> ImportBankAccountMovement(string userId, long bankAccountId, BankAccountMovementImportType importType, IFormFile file);
        List<Category> GetCategories(string userId);
        Category GetCategory(string userId, long id);
        Category GetCategoryDefault(string userId);
        void SaveCategory(string userId, Category category);
        void DeleteCategory(string userId, Category category);
    }
}
