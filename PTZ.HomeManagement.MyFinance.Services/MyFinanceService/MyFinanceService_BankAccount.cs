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
        public BankAccount GetBankAccountDefault(string userId)
        {
            ApplicationUser user = appRepo.GetUser(userId);
            return new BankAccount()
            {
                ApplicationUser = user,
                AccountType = AssetType.CurrentAccount,
                IsVisible = true
            };
        }

        public BankAccount GetBankAccount(string userId, long bankAccountId)
        {
            return myFinanceRepo.GetBankAccount(userId, bankAccountId);
        }
        public List<BankAccount> GetBankAccounts(string userId)
        {
            return myFinanceRepo.GetBankAccounts(userId);
        }
        public void SaveBankAccount(string userId, BankAccount bankAccount)
        {
            bankAccount.ApplicationUser = appRepo.GetUser(userId);
            myFinanceRepo.SaveBankAccount(userId, bankAccount);
            myFinanceRepo.CommitChanges();
        }
        public void DeleteBankAccount(string userId, BankAccount bankAccount)
        {
            myFinanceRepo.DeleteBankAccount(userId, bankAccount);
            myFinanceRepo.CommitChanges();
        }
    }
}