using Microsoft.EntityFrameworkCore;
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
    }
}
