using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.MyFinance.Models;

namespace PTZ.HomeManagement.MyFinance.Data
{
    public class MyFinanceRepositoryEF : IMyFinanceRepository
    {
        private readonly MyFinanceDbContext context;

        public MyFinanceRepositoryEF(IServiceProvider serviceProvider)
        {
            this.context = new MyFinanceDbContext(serviceProvider.GetRequiredService<DbContextOptions<MyFinanceDbContext>>());
            var lastAppliedMigration = this.context.Database.GetAppliedMigrations().LastOrDefault();
            var lastDefinedMigration = this.context.Database.GetMigrations().LastOrDefault();
            if (lastAppliedMigration != lastDefinedMigration)
            {
                this.context.Database.Migrate();
            }
        }

        public void CommitChanges()
        {
            this.context.SaveChanges();
        }

        public void DeleteBankAccount(string userId, BankAccount bankAccount)
        {
            var elementsToRemove = this.context.BankAccounts.Where(x => x.ApplicationUser.Id == userId && bankAccount.Id == x.Id);
            this.context.BankAccounts.RemoveRange(elementsToRemove);
        }

        public BankAccount GetBankAccount(string userId, int bankAccountId)
        {
            BankAccount account = this.context.BankAccounts.FirstOrDefault(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);

            var lastMovement = this.context.BankAccountMovements.Where(x => x.BankAccount.Id == account.Id).LastOrDefault();
            if (lastMovement != default(BankAccountMovement))
            {
                account.CurrentBalance = lastMovement.TotalBalanceAfterMovement;
            }

            return account;
        }

        public BankAccountMovement GetBankAccountMovement(string userId, int bankAccountId, int movementId)
        {
            return this.context.BankAccountMovements.FirstOrDefault(x => x.BankAccount.ApplicationUser.Id == userId && x.BankAccount.Id == bankAccountId && x.Id == movementId);
        }

        public List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified)
        {
            var list = this.context.BankAccountMovements.Where(x => x.BankAccount.ApplicationUser.Id == userId && x.BankAccount.Id == bankAccountId).Take(qtdOfMovements);
            switch (dateSortOrder)
            {
                case SortOrder.Ascending:
                    return list.OrderBy(x => x.MovementDate).ToList();
                default:
                    return list.OrderByDescending(x => x.MovementDate).ToList();
            }
        }

        public List<BankAccount> GetBankAccounts(string userId)
        {
            List<BankAccount> accounts = this.context.BankAccounts.Where(x => x.ApplicationUser.Id == userId).ToList();
            accounts.ForEach(account =>
            {
                var lastMovement = this.context.BankAccountMovements.Where(x => x.BankAccount.Id == account.Id).OrderByDescending(x => x.MovementDate).FirstOrDefault();
                if (lastMovement != default(BankAccountMovement))
                {
                    account.CurrentBalance = lastMovement.TotalBalanceAfterMovement;
                }
            });

            return accounts;
        }

        public void SaveBankAccount(string userId, BankAccount bankAccount)
        {
            this.context.Entry(bankAccount).State = bankAccount.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        public List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, DateTime startDate, DateTime endDate)
        {
            var list = this.context.BankAccountMovements.Where(x =>
                    x.BankAccount.ApplicationUser.Id == userId &&
                    x.BankAccount.Id == bankAccountId &&
                    x.MovementDate >= startDate &&
                    x.MovementDate <= endDate).ToList();

            return list;
        }

        public bool ExistsBankAccount(int bankAccountId, string userId)
        {
            return this.context.BankAccounts.Any(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);
        }

        public void SaveBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement)
        {
            bankAccountMovement.BankAccount = this.context.BankAccounts.Single(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);

            this.context.Entry(bankAccountMovement).State = bankAccountMovement.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        public void SaveBankAccountMovements(string userId, int bankAccountId, List<BankAccountMovement> list)
        {
            foreach (var bankAccountMovement in list)
            {
                bankAccountMovement.BankAccount = this.context.BankAccounts.Single(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);
                this.context.Entry(bankAccountMovement).State = bankAccountMovement.Id == 0 ? EntityState.Added : EntityState.Modified;
            }
        }

        public void DeleteBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement)
        {
            var elementsToRemove = this.context.BankAccountMovements.Where(x => x.BankAccount.Id == bankAccountId && x.BankAccount.ApplicationUser.Id == userId && x.Id == bankAccountMovement.Id);
            this.context.BankAccountMovements.RemoveRange(elementsToRemove);
        }

        public bool ExistsBankAccountMovements(BankAccountMovement item)
        {
            return this.context.BankAccountMovements.Any(x => x.GetHashCode() == item.GetHashCode());
        }
    }
}
