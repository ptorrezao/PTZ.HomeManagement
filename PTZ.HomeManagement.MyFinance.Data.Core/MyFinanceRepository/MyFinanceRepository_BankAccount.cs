using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.MyFinance.Models;

namespace PTZ.HomeManagement.MyFinance.Data
{
    public partial class MyFinanceRepositoryEF : IMyFinanceRepository
    {
        public BankAccount GetBankAccount(string userId, long bankAccountId)
        {
            BankAccount account = this.context.BankAccounts.FirstOrDefault(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);

            if (account != null)
            {
                var lastMovement = this.context.BankAccountMovements.Where(x => x.BankAccount.Id == account.Id).LastOrDefault();
                if (lastMovement != default(BankAccountMovement))
                {
                    account.CurrentBalance = lastMovement.TotalBalanceAfterMovement;
                }
            }

            return account;
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
        public void DeleteBankAccount(string userId, BankAccount bankAccount)
        {
            var elementsToRemove = this.context.BankAccounts.Where(x => x.ApplicationUser.Id == userId && bankAccount.Id == x.Id);
            this.context.BankAccounts.RemoveRange(elementsToRemove);
        }
        public void SaveBankAccount(string userId, BankAccount bankAccount)
        {
            this.context.Entry(bankAccount).State = bankAccount.Id == 0 ? EntityState.Added : EntityState.Modified;
        }
        public bool ExistsBankAccount(long bankAccountId, string userId)
        {
            return this.context.BankAccounts.Any(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);
        }
    }
}
