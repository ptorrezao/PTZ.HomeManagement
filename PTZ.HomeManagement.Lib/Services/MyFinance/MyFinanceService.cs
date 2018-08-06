using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Models.MyFinance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PTZ.HomeManagement.Services.MyFinance
{
    public class MyFinanceService : IMyFinanceService
    {
        private readonly ApplicationDbContext context;
        private BankAccountMovementImportFactory factory;

        public MyFinanceService(ApplicationDbContext context)
        {
            this.context = context;
        }


        public BankAccount GetBankAccountDefault(string userId)
        {
            ApplicationUser user = context.Users.FirstOrDefault(x => x.Id == userId);

            return new BankAccount()
            {
                ApplicationUser = user,
            };
        }
        public BankAccountMovement GetBankAccountMovementDefault(string userId, int bankAccountId)
        {
            BankAccount bankAccount = context.BankAccounts.FirstOrDefault(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);

            return new BankAccountMovement()
            {
                BankAccount = bankAccount,
                MovementDate = DateTime.Now,
                ValueDate = DateTime.Now
            };
        }

        public BankAccount GetBankAccount(string userId, int bankAccountId)
        {
            BankAccount account = context.BankAccounts.FirstOrDefault(x => x.ApplicationUser.Id == userId && x.Id == bankAccountId);

            var lastMovement = context.BankAccountMovements.Where(x => x.BankAccount.Id == account.Id).LastOrDefault();
            if (lastMovement != default(BankAccountMovement))
            {
                account.CurrentBalance = lastMovement.TotalBalanceAfterMovement;
            }

            return account;
        }
        public List<BankAccount> GetBankAccounts(string userId)
        {
            List<BankAccount> accounts = context.BankAccounts.Where(x => x.ApplicationUser.Id == userId).ToList();
            accounts.ForEach(account =>
            {
                var lastMovement = context.BankAccountMovements.Where(x => x.BankAccount.Id == account.Id).OrderByDescending(x => x.MovementDate).FirstOrDefault();
                if (lastMovement != default(BankAccountMovement))
                {
                    account.CurrentBalance = lastMovement.TotalBalanceAfterMovement;
                }
            });
            return accounts;
        }
        public void SaveBankAccount(string userId, BankAccount bankAccount)
        {
            bankAccount.ApplicationUser = context.Users.Single(x => x.Id == userId);
            context.Entry(bankAccount).State = bankAccount.Id == 0 ? EntityState.Added : EntityState.Modified;
            context.SaveChanges();
        }
        public void DeleteBankAccount(string userId, BankAccount bankAccount)
        {
            var elementsToRemove = context.BankAccounts.Where(x => x.ApplicationUser.Id == userId && bankAccount.Id == x.Id);
            context.BankAccounts.RemoveRange(elementsToRemove);
            context.SaveChanges();
        }

        public BankAccountMovement GetBankAccountMovement(string userId, int bankAccountId, int movementId)
        {
            return context.BankAccountMovements.FirstOrDefault(x => x.BankAccount.ApplicationUser.Id == userId && x.BankAccount.Id == bankAccountId && x.Id == movementId);
        }
        public List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified)
        {
            var list = context.BankAccountMovements.Where(x => x.BankAccount.ApplicationUser.Id == userId && x.BankAccount.Id == bankAccountId).Take(qtdOfMovements);
            switch (dateSortOrder)
            {
                case SortOrder.Ascending:
                    return list.OrderBy(x => x.MovementDate).ToList();
                default:
                    return list.OrderByDescending(x => x.MovementDate).ToList();
            }
        }
        public List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, DateTime startDate, DateTime endDate)
        {
            var list = context.BankAccountMovements.Where(x =>
                    x.BankAccount.ApplicationUser.Id == userId &&
                    x.BankAccount.Id == bankAccountId &&
                    x.MovementDate >= startDate &&
                    x.MovementDate <= endDate).ToList();

            return list;
        }
        public void SaveBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement)
        {
            if (context.BankAccounts.Any(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId))
            {
                bankAccountMovement.BankAccount = context.BankAccounts.Single(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);
                context.Entry(bankAccountMovement).State = bankAccountMovement.Id == 0 ? EntityState.Added : EntityState.Modified;
                context.SaveChanges();
            }
        }
        public List<BankAccountMovement> SaveBankAccountMovements(string userId, int bankAccountId, List<BankAccountMovement> list)
        {
            if (context.BankAccounts.Any(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId))
            {
                foreach (var bankAccountMovement in list)
                {
                    bankAccountMovement.BankAccount = context.BankAccounts.Single(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);
                    context.Entry(bankAccountMovement).State = bankAccountMovement.Id == 0 ? EntityState.Added : EntityState.Modified;
                }

                context.SaveChanges();
            }

            return list;
        }
        public void DeleteBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement)
        {
            var elementsToRemove = context.BankAccountMovements.Where(x => x.BankAccount.Id == bankAccountId && x.BankAccount.ApplicationUser.Id == userId && x.Id == bankAccountMovement.Id);
            context.BankAccountMovements.RemoveRange(elementsToRemove);
            context.SaveChanges();
        }

        public List<BankAccountMovement> ImportBankAccountMovement(string userId, int bankAccountId, BankAccountMovementImportType importType, IFormFile file)
        {
            factory = factory ?? new BankAccountMovementImportFactory();

            BankAccountMovementImport import = factory.GetBankAccountMovementImport(importType);
            List<BankAccountMovement> lines = import.GetBankAccountMovements(file);
            List<BankAccountMovement> linesToRemove = new List<BankAccountMovement>();
            foreach (var item in lines)
            {
                if (context.BankAccountMovements.Any(x => x.GetHashCode() == item.GetHashCode()))
                {
                    linesToRemove.Add(item);
                }
            }
            lines.RemoveAll(x => linesToRemove.Any(q => q.GetHashCode() == x.GetHashCode()));
            return this.SaveBankAccountMovements(userId, bankAccountId, lines);
        }
    }
}
