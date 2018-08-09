using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.MyFinance.Enums;
using PTZ.HomeManagement.MyFinance.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.MyFinance.Data
{
    public class MyFinanceDbContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountMovement> BankAccountMovements { get; set; }
        
        public MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options)
            : base(options)
        {
        }

        public EntityState GetContextState(ContextState state)
        {
            switch (state)
            {
                case ContextState.Detached:
                    return EntityState.Detached;
                case ContextState.Unchanged:
                    return EntityState.Unchanged;
                case ContextState.Deleted:
                    return EntityState.Deleted;
                case ContextState.Modified:
                    return EntityState.Modified;
                case ContextState.Added:
                default:
                    return EntityState.Added;
            }
        }
    }
}
