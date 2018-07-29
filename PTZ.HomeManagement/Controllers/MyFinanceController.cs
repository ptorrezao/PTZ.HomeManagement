using Microsoft.AspNetCore.Mvc;
using PTZ.HomeManagement.Models.MyFinanceViewModels;
using PTZ.HomeManagement.Models.MyFinanceViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Controllers
{
    public class MyFinanceController : Controller
    {
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

        public IActionResult ListAccounts()
        {
            AccountListViewModel lvm = new AccountListViewModel();
            lvm.Items.Add(new AccountListItemViewModel() { Id = 1, Name = "CGD" });
            lvm.Items.Add(new AccountListItemViewModel() { Id = 2, Name = "BPI" });
            return View(lvm);
        }

        public IActionResult AddAccount()
        {
            AccountViewModel lvm = new AccountViewModel();
            return View(lvm);
        }

        public IActionResult EditAccount()
        {
            return View();
        }

        public IActionResult DeleteAccount()
        {
            return View();
        }

        public IActionResult ListMovements()
        {
            return View();
        }

        public IActionResult ImportMovements()
        {
            return View();
        }
    }
}
