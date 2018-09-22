using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PTZ.HomeManagement.MyFinance.Models;

namespace PTZ.HomeManagement.MyFinance.Data
{
    public partial class MyFinanceRepositoryEF : IMyFinanceRepository
    {
        private readonly MyFinanceDbContext context;

        public MyFinanceRepositoryEF(IServiceProvider serviceProvider)
        {
            try
            {
                DbContextOptions<MyFinanceDbContext> options = serviceProvider.GetRequiredService<DbContextOptions<MyFinanceDbContext>>();
                this.context = new MyFinanceDbContext(options);

                if (!options.Extensions.Any(x => x.GetType() == typeof(InMemoryOptionsExtension)) &&
                    (!this.context.Database.GetAppliedMigrations().Any(x => x == this.context.Database.GetMigrations().LastOrDefault())))
                {
                    this.context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<MyFinanceRepositoryEF>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        public void CommitChanges()
        {
            this.context.SaveChanges();
        }

        public void SetCategoriesToBankAccountMovement(string userId, long bankAccountMovementId, List<long> categories)
        {
            categories = categories ?? new List<long>();
            if (context.BankAccountMovements.Any(x => x.Id == bankAccountMovementId))
            {
                var bankMovement = context.BankAccountMovements.First(x => x.Id == bankAccountMovementId);
                if (context.CategoriesBankAccountMovements.Any(q => q.BankAccountMovementId == bankAccountMovementId && !categories.Contains(q.CategoryId)))
                {
                    var categoriesToRemove = context.CategoriesBankAccountMovements.Where(q => q.BankAccountMovementId == bankAccountMovementId && !categories.Contains(q.CategoryId));
                    context.CategoriesBankAccountMovements.RemoveRange(categoriesToRemove);
                }

                foreach (long categoryId in categories)
                {
                    if (categoryId > 0 &&
                        context.Categories.Any(x => x.Id == categoryId) &&
                        !context.CategoriesBankAccountMovements.Any(x => x.CategoryId == categoryId && x.BankAccountMovementId == bankAccountMovementId))
                    {
                        context.CategoriesBankAccountMovements.Add(new CategoryBankAccountMovement()
                        {
                            BankAccountMovement = bankMovement,
                            BankAccountMovementId = bankMovement.Id,
                            CategoryId = categoryId,
                            Category = context.Categories.First(x => x.Id == categoryId),
                        });
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
