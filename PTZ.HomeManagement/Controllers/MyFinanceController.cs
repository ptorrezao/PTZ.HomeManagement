using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Models.MyFinance;
using PTZ.HomeManagement.Models.MyFinanceViewModels;
using PTZ.HomeManagement.Models.MyFinanceViewModels.Enums;
using PTZ.HomeManagement.Services.MyFinance;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace PTZ.HomeManagement.Controllers
{
    public class MyFinanceController : Controller
    {
        private readonly IMyFinanceService _myFinanceService;
        private readonly IHostingEnvironment _hostingEnv;

        [TempData]
        public string StatusMessage { get; set; }

        public MyFinanceController(
            IMyFinanceService myFinanceService,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hostingEnv)
        {
            _myFinanceService = myFinanceService;
            _hostingEnv = hostingEnv;
        }

        #region Movements
        #endregion

        #region Accounts
        public IActionResult ListAccounts()
        {
            List<BankAccount> bankAccounts = _myFinanceService.GetBankAccounts(User.GetUserId());
            return View(Mapper.Map<AccountListViewModel>(bankAccounts));
        }

        public IActionResult AddOrEditAccount(int? id)
        {
            BankAccount bankAccount = id.HasValue ? _myFinanceService.GetBankAccount(User.GetUserId(), id.Value) : _myFinanceService.GetBankAccountDefault(User.GetUserId());
            return View(Mapper.Map<AccountViewModel>(bankAccount));
        }

        [HttpPost]
        public IActionResult AddOrEditAccount(AccountViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                _myFinanceService.SaveBankAccount(User.GetUserId(), Mapper.Map<BankAccount>(lvm));

                return RedirectToAction(nameof(ListAccounts));
            }

            return View(lvm);
        }

        public IActionResult DeleteAccount(int id)
        {
            BankAccount bankAccount = _myFinanceService.GetBankAccount(User.GetUserId(), id);
            return View(Mapper.Map<AccountViewModel>(bankAccount));
        }

        [HttpPost]
        public IActionResult DeleteAccount(AccountViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                _myFinanceService.DeleteBankAccount(User.GetUserId(), Mapper.Map<BankAccount>(lvm));

                return RedirectToAction(nameof(ListAccounts));
            }

            return View(lvm);
        }
        #endregion

        #region Movements
        public IActionResult ListMovements(int bankAccountId)
        {
            BankAccount bankAccount = _myFinanceService.GetBankAccount(User.GetUserId(), bankAccountId);
            List<BankAccountMovement> movements = _myFinanceService.GetBankAccountMovements(User.GetUserId(), bankAccountId);

            AccountMovementListViewModel lvm = Mapper.Map<AccountMovementListViewModel>(bankAccount);
            lvm.Items = Mapper.Map<List<AccountMovementListItemViewModel>>(movements);
            return View(lvm);
        }

        public IActionResult AddOrEditMovement(int bankAccountId, int? id)
        {
            BankAccountMovement movement = id.HasValue ? _myFinanceService.GetBankAccountMovement(User.GetUserId(), bankAccountId, id.Value) : _myFinanceService.GetBankAccountMovementDefault(User.GetUserId(), bankAccountId);

            return View(Mapper.Map<AccountMovementViewModel>(movement));
        }

        [HttpPost]
        public IActionResult AddOrEditMovement(int bankAccountId, AccountMovementViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                _myFinanceService.SaveBankAccountMovement(User.GetUserId(), bankAccountId, Mapper.Map<BankAccountMovement>(lvm));

                return RedirectToAction(nameof(ListMovements), new { bankAccountId });
            }

            return View(lvm);
        }


        public IActionResult DeleteMovement(int bankAccountId, int? id)
        {
            BankAccountMovement movement = _myFinanceService.GetBankAccountMovement(User.GetUserId(), bankAccountId, id.Value);
            return View(Mapper.Map<AccountMovementViewModel>(movement));
        }

        [HttpPost]
        public IActionResult DeleteMovement(int bankAccountId, AccountMovementViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                _myFinanceService.DeleteBankAccountMovement(User.GetUserId(), bankAccountId, Mapper.Map<BankAccountMovement>(lvm));

                return RedirectToAction(nameof(ListMovements), new { bankAccountId });
            }

            return View(lvm);
        }
        #endregion

        public IActionResult Dashboard()
        {
            DashboardViewModel vm = new DashboardViewModel();
            vm.DoughnutChart.Assets.Add(new DashboardAccountViewModel() { Amount = 129.37M, AssetType = AssetType.DepositsAccount, AccountNumber = "0705013734600", AccountTitle = "Conta Pessoal", Bank = Bank.CGD });
            vm.DoughnutChart.Assets.Add(new DashboardAccountViewModel() { Amount = 975.19M, AssetType = AssetType.DepositsAccount, AccountNumber = "0156017664030", AccountTitle = "Conta Casa", Bank = Bank.CGD });
            vm.DoughnutChart.Assets.Add(new DashboardAccountViewModel() { Amount = 10M, AssetType = AssetType.SavingsAccount, AccountNumber = "0705013734261", AccountTitle = "Poupança", Bank = Bank.CGD });
            vm.DoughnutChart.Assets.Add(new DashboardAccountViewModel() { Amount = 4226.66M, AssetType = AssetType.DepositsAccount, AccountNumber = "4428997000001", AccountTitle = "Conta Ordem", Bank = Bank.BPI });
            vm.DoughnutChart.Assets.Add(new DashboardAccountViewModel() { Amount = 501.54M, AssetType = AssetType.FundsAccount, AccountNumber = "9-4428997.128.001", AccountTitle = "Poupança", Bank = Bank.BPI });
            vm.DoughnutChart.Assets.Add(new DashboardAccountViewModel() { Amount = 686.84M, AssetType = AssetType.RetirementSavingsAccount, AccountNumber = "9-4428997.000.001", AccountTitle = "PPR Ana", Bank = Bank.BPI });
            vm.DoughnutChart.Assets.Add(new DashboardAccountViewModel() { Amount = 686.84M, AssetType = AssetType.RetirementSavingsAccount, AccountNumber = "9-4428997.000.001", AccountTitle = "PPR Pedro", Bank = Bank.BPI });

            var qtdDays = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1).Day;

            for (int i = 1; i < qtdDays + 1; i++)
            {
                vm.BarChart.Movements.Add(new DashboardMovementViewModel()
                {
                    Day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i),
                    Amount = 686.84M + i,
                    AssetType = AssetType.RetirementSavingsAccount,
                    AccountNumber = "9-4428997.000.001",
                    AccountTitle = "PPR Pedro",
                    Bank = Bank.BPI
                });
            }
            for (int i = 1; i < qtdDays + 1; i++)
            {
                vm.BarChart.Movements.Add(new DashboardMovementViewModel()
                {
                    Day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i),
                    Amount = 286.84M + i,
                    AssetType = AssetType.DepositsAccount,
                    AccountNumber = "9-4428997.000.001",
                    AccountTitle = "Ordem",
                    Bank = Bank.CGD
                });
            }
            return View(vm);
        }

        public IActionResult ImportMovements(int bankAccountId)
        {
            return View(new AccountMovementImportViewModel(bankAccountId));
        }

        [HttpPost]
        public IActionResult ImportMovements(AccountMovementImportViewModel import, IFormFile file)
        {
            if (ModelState.IsValid && file.Length > 0)
            {
                List<BankAccountMovement> list = _myFinanceService.ImportBankAccountMovement(User.GetUserId(), import.BankAccountId, import.ImportType, file);

                import.Items = Mapper.Map<List<AccountMovementReviewListItemViewModel>>(list);

                return View("ImportMovementsReview", import);
            }

            return RedirectToAction(nameof(ImportMovements), new { import.BankAccountId });
        }
    }
}
