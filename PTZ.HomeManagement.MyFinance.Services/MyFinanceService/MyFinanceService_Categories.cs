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
        public Category GetCategoryDefault(string userId)
        {
            ApplicationUser user = appRepo.GetUser(userId);
            return new Category()
            {
                ApplicationUser = user,
            };
        }

        public List<Category> GetCategories(string userId)
        {
            return myFinanceRepo.GetCategories(userId);
        }

        public Category GetCategory(string userId, long id)
        {
            return myFinanceRepo.GetCategory(userId, id);
        }
        public void SaveCategory(string userId, Category category)
        {
            category.ApplicationUser = appRepo.GetUser(userId);
            myFinanceRepo.SaveCategory(userId, category);
            myFinanceRepo.CommitChanges();
        }

        public void DeleteCategory(string userId, Category category)
        {
            myFinanceRepo.DeleteCategory(userId, category);
            myFinanceRepo.CommitChanges();
        }
    }
}