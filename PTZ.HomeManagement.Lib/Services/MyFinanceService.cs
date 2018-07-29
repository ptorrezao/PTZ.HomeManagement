using Microsoft.EntityFrameworkCore;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Models.MyFinance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PTZ.HomeManagement.Services
{
    public class MyFinanceService : IMyFinanceService
    {
        private readonly ApplicationDbContext context;

        public MyFinanceService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void DeleteBankAccount(string userId, BankAccount bankAccount)
        {
            var elementsToRemove = context.BankAccounts.Where(x => x.ApplicationUser.Id == userId && bankAccount.Id == x.Id);
            context.BankAccounts.RemoveRange(elementsToRemove);
            context.SaveChanges();
        }

        public BankAccount GetBankAccount(string userId, int bankAccountId)
        {
            return context.BankAccounts.FirstOrDefault(x => x.ApplicationUser.Id == userId && x.Id == bankAccountId);
        }

        public BankAccount GetBankAccountDefault(string userId)
        {
            ApplicationUser user = context.Users.FirstOrDefault(x => x.Id == userId);

            return new BankAccount()
            {
                ApplicationUser = user,
            };
        }

        public List<BankAccount> GetBankAccounts(string userId)
        {
            return context.BankAccounts.Where(x => x.ApplicationUser.Id == userId).ToList();
        }

        public void SaveBankAccount(string userId, BankAccount bankAccount)
        {
            bankAccount.ApplicationUser = context.Users.Single(x => x.Id == userId);
            context.Entry(bankAccount).State = bankAccount.Id == 0 ? EntityState.Added : EntityState.Modified;
            context.SaveChanges();
        }
    }
}
