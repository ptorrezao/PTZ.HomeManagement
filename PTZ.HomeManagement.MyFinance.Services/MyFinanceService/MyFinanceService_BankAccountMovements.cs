using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.MyFinance.Models;
using PTZ.HomeManagement.MyFinance.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using PTZ.HomeManagement.MyFinance.Data;
using PTZ.HomeManagement.Core.Data;

namespace PTZ.HomeManagement.MyFinance
{
    public partial class MyFinanceService : IMyFinanceService
    {
        public BankAccountMovement GetBankAccountMovementDefault(string userId, long bankAccountId)
        {
            BankAccount bankAccount = myFinanceRepo.GetBankAccount(userId, bankAccountId);

            return new BankAccountMovement()
            {
                BankAccount = bankAccount,
                MovementDate = DateTime.Now,
                ValueDate = DateTime.Now
            };
        }
        public BankAccountMovement GetBankAccountMovement(string userId, long bankAccountId, int movementId)
        {
            return myFinanceRepo.GetBankAccountMovement(userId, bankAccountId, movementId);
        }
        public List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified)
        {
            return myFinanceRepo.GetBankAccountMovements(userId, bankAccountId, qtdOfMovements, dateSortOrder);
        }
        public List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, DateTime startDate, DateTime endDate)
        {
            return myFinanceRepo.GetBankAccountMovements(userId, bankAccountId, startDate, endDate);
        }
        public void SaveBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement)
        {
            if (myFinanceRepo.ExistsBankAccount(bankAccountId, userId))
            {
                myFinanceRepo.SaveBankAccountMovement(userId, bankAccountId, bankAccountMovement);
                myFinanceRepo.CommitChanges();
            }
        }
        public List<BankAccountMovement> SaveBankAccountMovements(string userId, long bankAccountId, List<BankAccountMovement> list)
        {
            if (myFinanceRepo.ExistsBankAccount(bankAccountId, userId))
            {
                myFinanceRepo.SaveBankAccountMovements(userId, bankAccountId, list);
                myFinanceRepo.CommitChanges();
            }

            return list;
        }
        public void DeleteBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement)
        {

            myFinanceRepo.DeleteBankAccountMovement(userId, bankAccountId, bankAccountMovement);
            myFinanceRepo.CommitChanges();
        }

        public List<BankAccountMovement> ImportBankAccountMovement(string userId, long bankAccountId, BankAccountMovementImportType importType, IFormFile file)
        {
            factory = factory ?? new BankAccountMovementImportFactory();

            BankAccountMovementImport import = factory.GetBankAccountMovementImport(importType);
            List<BankAccountMovement> lines = import.GetBankAccountMovements(file);
            List<BankAccountMovement> linesToRemove = new List<BankAccountMovement>();
            foreach (var item in lines)
            {
                if (myFinanceRepo.ExistsBankAccountMovements(bankAccountId, item))
                {
                    linesToRemove.Add(item);
                }
            }
            lines.RemoveAll(x => linesToRemove.Any(q => q.GetHashCode() == x.GetHashCode()));
            return lines;
        }

        public void SetCategoriesToBankAccountMovement(string userId, long bankAccountMovementId, List<long> categories)
        {
            myFinanceRepo.SetCategoriesToBankAccountMovement(userId, bankAccountMovementId, categories);
        }

        public List<BankAccountMovement> GetBankAccountMovementsWithoutCategories(string userId)
        {
            return myFinanceRepo.GetBankAccountMovements(userId, new List<Category>());
        }

    }
}