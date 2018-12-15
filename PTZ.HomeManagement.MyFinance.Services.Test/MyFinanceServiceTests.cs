using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using PTZ.HomeManagement.Core.Data;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.MyFinance.Data;
using PTZ.HomeManagement.MyFinance.Models;
using Xunit;

namespace PTZ.HomeManagement.MyFinance.Services.Test
{
    public class MyFinanceServiceTests
    {
        private static string BankAccountGetMovements = "";
        private readonly IMyFinanceRepository repo;
        private readonly IApplicationRepository apprepo;
        private readonly MyFinanceService myFinanceService;

        private long bankAccountIdDelete = 0;

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

            InitBanAccounts();
        }

        private void InitBanAccounts()
        {
            var user = apprepo.GetUsers(null).FirstOrDefault();
            var userId = user.Id;
            var bankAccount = this.myFinanceService.GetBankAccountDefault(userId);

            bankAccount.IBAN = "PT50000201231234567890111";
            bankAccount.AccountType = AssetType.CurrentAccount;
            bankAccount.Bank = Bank.CGD;
            bankAccount.Color = "#00FF11";
            bankAccount.IsVisible = true;
            bankAccount.Name = nameof(BankAccount_Update);
            this.myFinanceService.SaveBankAccount(userId, bankAccount);

            var bankAccountToDelete = this.myFinanceService.GetBankAccountDefault(userId);

            bankAccountToDelete.IBAN = "PT5000020121231234567890111";
            bankAccountToDelete.AccountType = AssetType.CurrentAccount;
            bankAccountToDelete.Bank = Bank.CGD;
            bankAccountToDelete.Color = "#00FF11";
            bankAccountToDelete.IsVisible = true;
            bankAccountToDelete.Name = nameof(BankAccount_Delete);
            this.myFinanceService.SaveBankAccount(userId, bankAccountToDelete);

            var savedAccounts = this.myFinanceService.GetBankAccounts(userId);
            bankAccountIdDelete = savedAccounts.FirstOrDefault(x => x.Name == nameof(BankAccount_Delete)).Id;

            var listBankAccount1 = this.myFinanceService.GetBankAccountDefault(userId);

            listBankAccount1.IBAN = "PT50000201231234567890222";
            listBankAccount1.AccountType = AssetType.CurrentAccount;
            listBankAccount1.Bank = Bank.BPI;
            listBankAccount1.Color = "#11FF11";
            listBankAccount1.IsVisible = true;
            listBankAccount1.Name = nameof(BankAccount_List);
            this.myFinanceService.SaveBankAccount(userId, listBankAccount1);


            var listBankAccount2 = this.myFinanceService.GetBankAccountDefault(userId);

            listBankAccount2.IBAN = "PT50000201231234567890333";
            listBankAccount2.AccountType = AssetType.CurrentAccount;
            listBankAccount2.Bank = Bank.BPI;
            listBankAccount2.Color = "#11FF11";
            listBankAccount2.IsVisible = true;
            listBankAccount2.Name = nameof(BankAccount_List);
            this.myFinanceService.SaveBankAccount(userId, listBankAccount2);
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
            Assert.Equal(0, bankAccount.CurrentBalance);
            Assert.Equal(bankAccount.ApplicationUser.Id, userId);
            Assert.Equal(AssetType.CurrentAccount, bankAccount.AccountType);
            Assert.True(bankAccount.IsVisible);
        }

        [Fact]
        public void BankAccount_AddNew()
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

            bankAccount.IBAN = "PT50000201231234567890154";
            bankAccount.AccountType = AssetType.RetirementSavingsAccount;
            bankAccount.Bank = Bank.BPI;
            bankAccount.Color = "#00FF00";
            bankAccount.IsVisible = true;
            bankAccount.Name = nameof(BankAccount_AddNew);
            this.myFinanceService.SaveBankAccount(userId, bankAccount);

            var savedAccounts = this.myFinanceService.GetBankAccounts(userId);

            var savedAccount = savedAccounts.FirstOrDefault(x => x.Name == nameof(BankAccount_AddNew));

            Assert.NotNull(savedAccount);
            Assert.NotNull(savedAccount.ApplicationUser);
            Assert.Equal("PT50000201231234567890154", savedAccount.IBAN);
            Assert.Equal(AssetType.RetirementSavingsAccount, savedAccount.AccountType);
            Assert.Equal(Bank.BPI, savedAccount.Bank);
            Assert.Equal("#00FF00", savedAccount.Color);
            Assert.Equal(0, savedAccount.CurrentBalance);
            Assert.True(savedAccount.IsVisible);
            Assert.Equal(nameof(BankAccount_AddNew), savedAccount.Name);
        }

        [Fact]
        public void BankAccount_Update()
        {
            var user = apprepo.GetUsers(null).FirstOrDefault();
            Assert.NotNull(user);
            var userId = user.Id;
            var savedAccounts = this.myFinanceService.GetBankAccounts(userId);

            var bankAccount = savedAccounts.FirstOrDefault(x => x.Name == nameof(BankAccount_Update));

            bankAccount.IBAN = "PT10000201231234567890111";
            bankAccount.AccountType = AssetType.FundsAccount;
            this.myFinanceService.SaveBankAccount(userId, bankAccount);

            savedAccounts = this.myFinanceService.GetBankAccounts(userId);
            bankAccount = savedAccounts.FirstOrDefault(x => x.Name == nameof(BankAccount_Update));
            Assert.NotNull(bankAccount);
            Assert.NotNull(bankAccount.ApplicationUser);
            Assert.Equal("PT10000201231234567890111", bankAccount.IBAN);
            Assert.Equal(AssetType.FundsAccount, bankAccount.AccountType);
        }

        [Fact]
        public void BankAccount_Delete()
        {
            var user = apprepo.GetUsers(null).FirstOrDefault();
            Assert.NotNull(user);
            var userId = user.Id;
            var bankAccount = this.myFinanceService.GetBankAccount(userId, bankAccountIdDelete);

            this.myFinanceService.DeleteBankAccount(userId, bankAccount);

            bankAccount = this.myFinanceService.GetBankAccount(userId, bankAccountIdDelete);
            Assert.Null(bankAccount);
        }

        [Fact]
        public void BankAccount_List()
        {
            var user = apprepo.GetUsers(null).FirstOrDefault();
            Assert.NotNull(user);
            var userId = user.Id;
            Assert.True(this.myFinanceService.GetBankAccounts(userId).Count > 0);
        }
    }
}
