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
            return this.context.BankAccountMovements.FirstOrDefault(x => x.BankAccount.ApplicationUser.Id == userId && x.BankAccount.Id == bankAccountId && x.Id == movementId);
        }

        public List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified)
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

        public List<BankAccountMovement> GetBankAccountMovements(string userId, long bankAccountId, DateTime startDate, DateTime endDate)
        {
            var list = this.context.BankAccountMovements.Where(x =>
                    x.BankAccount.ApplicationUser.Id == userId &&
                    x.BankAccount.Id == bankAccountId &&
                    x.MovementDate >= startDate &&
                    x.MovementDate <= endDate).ToList();

            return list;
        }

        public void SaveBankAccountMovement(string userId, long bankAccountId, BankAccountMovement bankAccountMovement)
        {
            bankAccountMovement.BankAccount = this.context.BankAccounts.Single(x => x.Id == bankAccountId && x.ApplicationUser.Id == userId);

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

        public bool ExistsBankAccountMovements(BankAccountMovement item)
        {
            return this.context.BankAccountMovements.Include(x => x.BankAccount).Any(x => x.GetHashCode() == item.GetHashCode());
        }
    }
}
