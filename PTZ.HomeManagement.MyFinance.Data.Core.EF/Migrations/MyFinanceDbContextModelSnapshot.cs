﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.MyFinance.Data;
using System;

namespace PTZ.HomeManagement.MyFinance.Data.EF.Migrations
{
    [DbContext(typeof(MyFinanceDbContext))]
    partial class MyFinanceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("Relational:Sequence:.BankAccount", "'BankAccount', '', '1', '1', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.BankAccountMovement", "'BankAccountMovement', '', '1', '1', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.Category", "'Category', '', '1', '1', '', '', 'Int64', 'False'");

            modelBuilder.Entity("PTZ.HomeManagement.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<bool>("RequirePasswordChange");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PTZ.HomeManagement.MyFinance.Models.BankAccount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("nextval('\"BankAccount\"')");

                    b.Property<int>("AccountType");

                    b.Property<string>("ApplicationUserId");

                    b.Property<int>("Bank");

                    b.Property<string>("Color");

                    b.Property<string>("IBAN");

                    b.Property<bool>("IsVisible");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("PTZ.HomeManagement.MyFinance.Models.BankAccountMovement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("nextval('\"BankAccountMovement\"')");

                    b.Property<decimal>("Amount");

                    b.Property<long?>("BankAccountId");

                    b.Property<string>("Description");

                    b.Property<DateTime>("MovementDate");

                    b.Property<decimal>("TotalBalanceAfterMovement");

                    b.Property<DateTime>("ValueDate");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.ToTable("BankAccountMovements");
                });

            modelBuilder.Entity("PTZ.HomeManagement.MyFinance.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("nextval('\"Category\"')");

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Color");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PTZ.HomeManagement.MyFinance.Models.BankAccount", b =>
                {
                    b.HasOne("PTZ.HomeManagement.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("PTZ.HomeManagement.MyFinance.Models.BankAccountMovement", b =>
                {
                    b.HasOne("PTZ.HomeManagement.MyFinance.Models.BankAccount", "BankAccount")
                        .WithMany("Movements")
                        .HasForeignKey("BankAccountId");
                });

            modelBuilder.Entity("PTZ.HomeManagement.MyFinance.Models.Category", b =>
                {
                    b.HasOne("PTZ.HomeManagement.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
