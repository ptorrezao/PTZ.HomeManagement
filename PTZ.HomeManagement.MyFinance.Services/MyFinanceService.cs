using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.MyFinance.Models;
using PTZ.HomeManagement.MyFinance.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using PTZ.HomeManagement.MyFinance.Data;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.MyFinance;

namespace PTZ.HomeManagement.MyFinance
{
    public class MyFinanceService : IMyFinanceService
    {
        private readonly IMyFinanceRepository myFinanceRepo;
        private readonly ApplicationDbContext dbContext;

        private BankAccountMovementImportFactory factory;

        public MyFinanceService(IMyFinanceRepository repo,
            ApplicationDbContext dbContext)
        {
            this.myFinanceRepo = repo;
            this.dbContext = dbContext;
        }

        public BankAccount GetBankAccountDefault(string userId)
        {
            ApplicationUser user = dbContext.Users.FirstOrDefault(x => x.Id == userId);

            return new BankAccount()
            {
                ApplicationUser = user,
            };
        }
        public BankAccountMovement GetBankAccountMovementDefault(string userId, int bankAccountId)
        {
            BankAccount bankAccount = myFinanceRepo.GetBankAccount(userId, bankAccountId);

            return new BankAccountMovement()
            {
                BankAccount = bankAccount,
                MovementDate = DateTime.Now,
                ValueDate = DateTime.Now
            };
        }

        public BankAccount GetBankAccount(string userId, int bankAccountId)
        {
            return myFinanceRepo.GetBankAccount(userId, bankAccountId);
        }
        public List<BankAccount> GetBankAccounts(string userId)
        {
            return myFinanceRepo.GetBankAccounts(userId);
        }
        public void SaveBankAccount(string userId, BankAccount bankAccount)
        {
            bankAccount.ApplicationUser = dbContext.Users.Single(x => x.Id == userId);
            myFinanceRepo.SaveBankAccount(userId, bankAccount);
            myFinanceRepo.CommitChanges();
        }
        public void DeleteBankAccount(string userId, BankAccount bankAccount)
        {
            myFinanceRepo.DeleteBankAccount(userId, bankAccount);
            myFinanceRepo.CommitChanges();
        }

        public BankAccountMovement GetBankAccountMovement(string userId, int bankAccountId, int movementId)
        {
            return myFinanceRepo.GetBankAccountMovement(userId, bankAccountId, movementId);
        }
        public List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified)
        {
            return myFinanceRepo.GetBankAccountMovements(userId, bankAccountId, qtdOfMovements, dateSortOrder);
        }
        public List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, DateTime startDate, DateTime endDate)
        {
            return myFinanceRepo.GetBankAccountMovements(userId, bankAccountId, startDate, endDate);
        }
        public void SaveBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement)
        {
            if (myFinanceRepo.ExistsBankAccount(bankAccountId, userId))
            {
                myFinanceRepo.SaveBankAccountMovement(userId, bankAccountId, bankAccountMovement);
                myFinanceRepo.CommitChanges();
            }
        }
        public List<BankAccountMovement> SaveBankAccountMovements(string userId, int bankAccountId, List<BankAccountMovement> list)
        {
            if (myFinanceRepo.ExistsBankAccount(bankAccountId, userId))
            {
                myFinanceRepo.SaveBankAccountMovements(userId, bankAccountId, list);
                myFinanceRepo.CommitChanges();
            }

            return list;
        }
        public void DeleteBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement)
        {

            myFinanceRepo.DeleteBankAccountMovement(userId, bankAccountId, bankAccountMovement);
            myFinanceRepo.CommitChanges();
        }

        public List<BankAccountMovement> ImportBankAccountMovement(string userId, int bankAccountId, BankAccountMovementImportType importType, IFormFile file)
        {
            factory = factory ?? new BankAccountMovementImportFactory();

            BankAccountMovementImport import = factory.GetBankAccountMovementImport(importType);
            List<BankAccountMovement> lines = import.GetBankAccountMovements(file);
            List<BankAccountMovement> linesToRemove = new List<BankAccountMovement>();
            foreach (var item in lines)
            {
                if (myFinanceRepo.ExistsBankAccountMovements(item))
                {
                    linesToRemove.Add(item);
                }
            }
            lines.RemoveAll(x => linesToRemove.Any(q => q.GetHashCode() == x.GetHashCode()));
            return this.SaveBankAccountMovements(userId, bankAccountId, lines);
        }
    }
}
