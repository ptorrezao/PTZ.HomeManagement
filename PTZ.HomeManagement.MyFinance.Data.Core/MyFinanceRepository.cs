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
        private readonly MyFinanceDbContext context;

        public MyFinanceRepositoryEF(IServiceProvider serviceProvider) 
        {
            DbContextOptions<MyFinanceDbContext> options = serviceProvider.GetRequiredService<DbContextOptions<MyFinanceDbContext>>();
            this.context = new MyFinanceDbContext(options);

            if (!options.Extensions.Any(x => x.GetType() == typeof(InMemoryOptionsExtension)))
            {
                var lastDefinedMigration = this.context.Database.GetMigrations().LastOrDefault();
                if (this.context.Database.GetAppliedMigrations().Any(x => x == lastDefinedMigration))
                {
                    this.context.Database.Migrate();
                }
            }
        }

        public void CommitChanges()
        {
            this.context.SaveChanges();
        }
    }
}
