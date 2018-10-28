using Microsoft.AspNetCore.Mvc;
using PTZ.HomeManagement.Models.MyFinanceViewModels;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.MyFinance.Models;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Components.MyFinance
{
    public class CategoriesDistribution : ViewComponent
    {
        private readonly IMyFinanceService _myFinanceService;

        public CategoriesDistribution(
            IMyFinanceService myFinanceService)
        {
            _myFinanceService = myFinanceService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            BarChartViewModel viewModel = await Task.Run(() =>
            {
                DateTime now = DateTime.Now;
                BarChartViewModel vm = new BarChartViewModel();
                vm.MinDate = DateTime.MaxValue;
                vm.MaxDate = DateTime.MinValue;
                List<BankAccount> bankAccounts = _myFinanceService.GetBankAccounts(User.GetUserId());

                bankAccounts.Where(x => x.IsVisible).ToList().ForEach(bankAccount =>
                {
                    var categories = _myFinanceService.GetCategories(User.GetUserId());
                    DateTime lastPaymentDay = now.Day > 24 ? new DateTime(now.Year, now.Month, 24) : new DateTime(now.Year, now.Month, 24).AddMonths(-1);
                    bankAccount.Movements = _myFinanceService.GetBankAccountMovements(User.GetUserId(), bankAccount.Id, lastPaymentDay, now);

                    foreach (var selectedCategory in categories)
                    {
                        bankAccount.Movements.GroupBy(x => x.BankAccount).ToList().ForEach(x =>
                        {
                            if (x.Key.Movements.Where(r => r.Categories != null).SelectMany(q => q.Categories).Any())
                            {
                                var items = x.Where(e => e.Categories != null && e.Categories.Any(r => r.CategoryId == selectedCategory.Id));

                                if (items.Any())
                                {
                                    var amout = -items.Sum(q => q.Amount);
                                    var minDate = items.Min(q => q.MovementDate);
                                    var maxDate = items.Max(q => q.MovementDate);
                                    if (amout > 0)
                                    {
                                        vm.MinDate = vm.MinDate > minDate ? minDate : vm.MinDate;
                                        vm.MaxDate = vm.MaxDate < maxDate ? maxDate : vm.MaxDate;

                                        vm.Items.Add(new BarChartItemViewModel()
                                        {
                                            Label = selectedCategory.Name,
                                            Color = bankAccount.Color,
                                            Value = amout,
                                            Group = bankAccount.Bank.ToString()
                                        });
                                    }
                                }
                            }
                        });
                    }
                });

                return vm;
            });

            return View(viewModel);
        }
    }
}
