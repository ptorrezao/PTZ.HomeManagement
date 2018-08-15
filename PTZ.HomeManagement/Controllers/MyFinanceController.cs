﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Models.MyFinanceViewModels;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.MyFinance.Models;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PTZ.HomeManagement.Controllers
{
    public class MyFinanceController : Controller
    {
        private readonly IMyFinanceService _myFinanceService;

        [TempData]
        public string StatusMessage { get; set; }

        public MyFinanceController(
            IMyFinanceService myFinanceService,
            UserManager<ApplicationUser> userManagerv)
        {
            _myFinanceService = myFinanceService;
        }

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
        #endregion

        #region Dashboard
        public IActionResult Dashboard()
        {
            DashboardViewModel vm = new DashboardViewModel();
            List<BankAccount> bankAccounts = _myFinanceService.GetBankAccounts(User.GetUserId());
            int graphLenght = 30;
            int graphLenghtIntoFuture = 1;

            bankAccounts.Where(x => x.IsVisible).ToList().ForEach(bankAccount =>
            {
                decimal lastKnownValue = 0;
                vm.DoughnutChart.Assets.Add(Mapper.Map<DashboardAccountViewModel>(bankAccount));

                bankAccount.Movements = _myFinanceService.GetBankAccountMovements(User.GetUserId(), bankAccount.Id, DateTime.Now.AddDays(-graphLenght), DateTime.Now);

                var currentDay = DateTime.Now.AddDays(-graphLenght).Date;
                for (int i = 1; i < graphLenght + graphLenghtIntoFuture; i++)
                {
                    currentDay = currentDay.AddDays(1);
                    var movement = bankAccount.Movements.FirstOrDefault(x => x.MovementDate.Date == currentDay);

                    if (movement != default(BankAccountMovement))
                    {
                        lastKnownValue = movement.TotalBalanceAfterMovement;
                    }

                    vm.BarChart.Movements.Add(new DashboardMovementViewModel()
                    {
                        Day = currentDay,
                        Amount = lastKnownValue,
                        AssetType = bankAccount.AccountType,
                        AccountNumber = bankAccount.IBAN,
                        AccountTitle = bankAccount.Name,
                        Bank = bankAccount.Bank,
                    });
                }
            });

            return View(vm);
        }
        #endregion

        #region Categories
        public IActionResult ListCategories()
        {
            List<Category> categories = _myFinanceService.GetCategories(User.GetUserId());
            return View(Mapper.Map<CategoryListViewModel>(categories));
        }

        public IActionResult AddOrEditCategory(int? id)
        {
            Category category = id.HasValue ? _myFinanceService.GetCategory(User.GetUserId(), id.Value) : _myFinanceService.GetCategoryDefault(User.GetUserId());
            return View(Mapper.Map<CategoryViewModel>(category));
        }

        [HttpPost]
        public IActionResult AddOrEditCategory(CategoryViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                _myFinanceService.SaveCategory(User.GetUserId(), Mapper.Map<Category>(lvm));

                return RedirectToAction(nameof(ListCategories));
            }

            return View(lvm);
        }

        public IActionResult DeleteCategory(int id)
        {
            Category category = _myFinanceService.GetCategory(User.GetUserId(), id);
            return View(Mapper.Map<CategoryViewModel>(category));
        }

        [HttpPost]
        public IActionResult DeleteCategory(CategoryViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                _myFinanceService.DeleteCategory(User.GetUserId(), Mapper.Map<Category>(lvm));

                return RedirectToAction(nameof(ListCategories));
            }

            return View(lvm);
        }
        #endregion
    }
}
