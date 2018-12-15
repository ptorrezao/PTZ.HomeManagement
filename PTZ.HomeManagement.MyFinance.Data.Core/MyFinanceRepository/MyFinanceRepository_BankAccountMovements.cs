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
        public BankAccountMovement GetBankAccountMovement(string userId, long bankAccountId, int movementId)
        {
            return this.context.BankAccountMovements
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.BankAccount.ApplicationUser.Id == userId && x.BankAccount.Id == bankAccountId && x.Id == movementId);
        }

        public List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified)
        {
            var list = this.context.BankAccountMovements.Include(x => x.Categories).ThenInclude(p => p.Category).Where(x => x.BankAccount.ApplicationUser.Id == userId && x.BankAccount.Id == bankAccountId);
            switch (dateSortOrder)
            {
                case SortOrder.Ascending:
                    return list.OrderBy(x => x.MovementDate).Take(qtdOfMovements).ToList();
                default:
                    return list.OrderByDescending(x => x.MovementDate).Take(qtdOfMovements).ToList();
            }
        }

        public List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, DateTime startDate, DateTime endDate)
        {
            var list = this.context.BankAccountMovements
                    .Include(x => x.Categories)
                    .Where(x => x.BankAccount.ApplicationUser.Id == userId &&
                    x.BankAccount.Id == bankAccountId &&
                    x.MovementDate >= startDate &&
                    x.MovementDate <= endDate).ToList();

            return list;
        }

        public void SaveBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement)
        {
            bankAccountMovement.BankAccount = this.context.BankAccounts.Single(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);
            this.context.Entry(bankAccountMovement.BankAccount).State = EntityState.Unchanged;

            if (bankAccountMovement.Categories != null && bankAccountMovement.Categories.Any())
            {
                bankAccountMovement.Categories.ForEach(x =>
                {
                    x.BankAccountMovement = bankAccountMovement;
                    x.Category = x.Category ?? context.Categories.First(q => q.Id == x.CategoryId);
                });
            }

            if (bankAccountMovement.Id == 0 &&
                bankAccountMovement.TotalBalanceAfterMovement == 0)
            {
                var lstMov = this.GetBankAccountMovements(userId, bankAccountId, 1, SortOrder.Ascending).FirstOrDefault();

                if (lstMov != null)
                {
                    bankAccountMovement.TotalBalanceAfterMovement = lstMov.TotalBalanceAfterMovement;
                }

                bankAccountMovement.TotalBalanceAfterMovement += bankAccountMovement.Amount;
            }
            this.context.Entry(bankAccountMovement).State = bankAccountMovement.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        public void SaveBankAccountMovements(string userId, long bankAccountId, List<BankAccountMovement> list)
        {
            foreach (var bankAccountMovement in list)
            {
                bankAccountMovement.BankAccount = this.context.BankAccounts.Single(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);
                this.context.Entry(bankAccountMovement).State = bankAccountMovement.Id == 0 ? EntityState.Added : EntityState.Modified;
            }
        }

        public void DeleteBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement)
        {
            var elementsToRemove = this.context.BankAccountMovements.Where(x => x.BankAccount.Id == bankAccountId && x.BankAccount.ApplicationUser.Id == userId && x.Id == bankAccountMovement.Id);
            this.context.BankAccountMovements.RemoveRange(elementsToRemove);
        }

        public bool ExistsBankAccountMovements(long bankAccountId, BankAccountMovement item)
        {
            return this.context.BankAccountMovements.Include(x => x.BankAccount).Any(x => x.GetHashCode() == item.GetHashCode() && x.BankAccount.Id == bankAccountId);
        }

        public List<BankAccountMovement> GetBankAccountMovements(string userId, List<Category> categories, int limit = 50)
        {
            var elements = this.context.BankAccountMovements
                                   .Include(x => x.Categories);

            if (categories != null && categories.Count > 0)
            {
                return elements.Where(x => x.BankAccount.ApplicationUser.Id == userId && categories.Any(q => x.Categories.Any(c => c.CategoryId == q.Id))).OrderByDescending(x => x.MovementDate).Take(limit).ToList();
            }
            else
            {
                return elements.Where(x => x.BankAccount.ApplicationUser.Id == userId && x.Categories.Count == 0).OrderByDescending(x => x.MovementDate).Take(limit).ToList();
            }

        }
    }
}
