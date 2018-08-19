using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using PTZ.HomeManagement.Core.Data;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.MyFinance.Data;
using Xunit;

namespace PTZ.HomeManagement.MyFinance.Services.Test
{
    public class MyFinanceServiceTests
    {
        private readonly IMyFinanceRepository repo;
        private readonly IApplicationRepository apprepo;
        private readonly MyFinanceService myFinanceService;

        public MyFinanceServiceTests()
        {
            var myFinanceDbContextOptions = new DbContextOptionsBuilder<MyFinanceDbContext>()
                .UseInMemoryDatabase(databaseName: "MyFinanceDb")
                .Options;

            var appDbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyFinanceDb")
                .Options;

            var mock = new Mock<IServiceProvider>();
            mock.Setup(p => p.GetService(typeof(DbContextOptions<MyFinanceDbContext>))).Returns(myFinanceDbContextOptions);
            mock.Setup(p => p.GetService(typeof(DbContextOptions<ApplicationDbContext>))).Returns(appDbContextOptions);
            var serviceProvider = mock.Object;

            this.repo = new MyFinanceRepositoryEF(serviceProvider);
            this.apprepo = new ApplicationDbRepository(serviceProvider);
            this.myFinanceService = new MyFinanceService(repo, apprepo);

            InitUsers();
        }

        private void InitUsers()
        {
            var testUser = new ApplicationUser()
            {
                Email = "test@example.com",
                UserName = "test@example.com",
                NormalizedEmail = "test@example.com",
                PasswordHash = ""
            };
            apprepo.SaveUser(testUser);
        }

        [Fact]
        public void BankAccount_CheckDefault()
        {
            var user = apprepo.GetUsers(null).FirstOrDefault();
            Assert.NotNull(user);
            var userId = user.Id;
            var bankAccount = this.myFinanceService.GetBankAccountDefault(userId);

            Assert.NotNull(bankAccount);
            Assert.NotNull(bankAccount.ApplicationUser);
            Assert.Equal(bankAccount.ApplicationUser.Id, userId);
            Assert.Equal(AssetType.CurrentAccount, bankAccount.AccountType);
            Assert.True(bankAccount.IsVisible);
        }
    }
}