﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using PTZ.HomeManagement.ExpirationReminder.Data.Core.EF;
using System;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core.EF.Migrations
{
    [DbContext(typeof(ExpirationReminderDbContext))]
    [Migration("20180923025250_AddReminder")]
    partial class AddReminder
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("Relational:Sequence:.Reminder", "'Reminder', '', '1', '1', '', '', 'Int64', 'False'");

            modelBuilder.Entity("PTZ.HomeManagement.ExpirationReminder.Core.Reminder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("nextval('\"Reminder\"')");

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<int>("ReminderType");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("PTZ.HomeManagement.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

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

            modelBuilder.Entity("PTZ.HomeManagement.ExpirationReminder.Core.Reminder", b =>
                {
                    b.HasOne("PTZ.HomeManagement.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
