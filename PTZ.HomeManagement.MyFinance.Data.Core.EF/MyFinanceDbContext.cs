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
        public DbSet<Category> Categories { get; set; }

        public MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.HasSequence<long>("BankAccount").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<BankAccount>(b =>
            {
                b.Property(o => o.Id).HasDefaultValueSql("nextval('\"BankAccount\"')");
                b.Property(o => o.Name).IsRequired();
                b.Ignore(o => o.CurrentBalance);
                b.HasKey(o => o.Id);
                b.Property<string>("ApplicationUserId");
                b.HasIndex("ApplicationUserId");
                b.ToTable("BankAccounts");
                b.HasOne(o => o.ApplicationUser).WithMany().HasForeignKey("ApplicationUserId");
            });

            modelBuilder.HasSequence<long>("BankAccountMovement").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<BankAccountMovement>(b =>
            {
                b.Property(o => o.Id).HasDefaultValueSql("nextval('\"BankAccountMovement\"')");
                b.HasKey(o => o.Id);
                b.HasIndex("BankAccountId");
                b.ToTable("BankAccountMovements");
                b.HasOne(o => o.BankAccount).WithMany(x => x.Movements).HasForeignKey("BankAccountId");
            });

            modelBuilder.HasSequence<long>("Category").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Category>(b =>
            {
                b.Property(o => o.Id).HasDefaultValueSql("nextval('\"Category\"')");
                b.HasKey(o => o.Id);
            });

            modelBuilder.Entity<CategoryBankAccountMovement>(b =>
            {
                b.HasKey(q => new { q.BankAccountMovementId, q.CategoryId });
                b.HasOne(q => q.Category).WithMany(q => q.Movements).HasForeignKey(x => x.CategoryId);
                b.HasOne(q => q.BankAccountMovement).WithMany(q => q.Categories).HasForeignKey(x => x.BankAccountMovementId);
            });
        }
    }
}
