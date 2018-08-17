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
        public List<Category> GetCategories(string userId)
        {
            return this.context.Categories.Where(x => x.ApplicationUser.Id == userId).ToList();
        }

        public Category GetCategory(string userId, long id)
        {
            Category category = this.context.Categories.FirstOrDefault(x => x.Id == id && x.ApplicationUser.Id == userId);

            return category;
        }

        public void SaveCategory(string userId, Category category)
        {
            this.context.Entry(category).State = category.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        public void DeleteCategory(string userId, Category category)
        {
            var elementsToRemove = this.context.Categories.Where(x => x.ApplicationUser.Id == userId && category.Id == x.Id);
            this.context.Categories.RemoveRange(elementsToRemove);
        }
    }
}
