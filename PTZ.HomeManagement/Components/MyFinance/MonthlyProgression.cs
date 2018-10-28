using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PTZ.HomeManagement.Models.MyFinanceViewModels;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.MyFinance.Models;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Components.MyFinance
{
    public class MonthlyProgression : ViewComponent
    {
        private readonly IMyFinanceService _myFinanceService;
 
        public MonthlyProgression(
            IMyFinanceService myFinanceService)
        {
            _myFinanceService = myFinanceService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int graphLenght = 20;
            int graphLenghtIntoFuture = 1;

            LineChartViewModel monthlyProgression = await Task.Run(() =>
            {
                LineChartViewModel vm = new LineChartViewModel();
                List<BankAccount> bankAccounts = _myFinanceService.GetBankAccounts(User.GetUserId());

                bankAccounts.Where(x => x.IsVisible).ToList().ForEach(bankAccount =>
                {
                    decimal lastKnownValue = 0;

                    bankAccount.Movements = _myFinanceService.GetBankAccountMovements(User.GetUserId(), bankAccount.Id, DateTime.Now.AddDays(-graphLenght), DateTime.Now);

                    lastKnownValue = GetLastKnownValue(bankAccount, lastKnownValue);

                    var currentDay = DateTime.Now.AddDays(-graphLenght).Date;
                    for (int i = 1; i < graphLenght + graphLenghtIntoFuture; i++)
                    {
                        currentDay = currentDay.AddDays(1);
                        var movement = bankAccount.Movements.FirstOrDefault(x => x.MovementDate.Date == currentDay);

                        lastKnownValue = UpdateLastKnownValue(bankAccount, lastKnownValue, movement);

                        vm.Movements.Add(new LineChartItemViewModel()
                        {
                            XAxis = currentDay.ToShortDateString(),
                            Amount = lastKnownValue,
                            Color = bankAccount.Color,
                            Group = bankAccount.Bank.ToString(),
                        });
                    }
                });

                return vm;
            });

            return View(monthlyProgression);
        }

        private decimal UpdateLastKnownValue(BankAccount bankAccount, decimal lastKnownValue, BankAccountMovement movement)
        {
            if (movement != default(BankAccountMovement))
            {
                lastKnownValue = movement.TotalBalanceAfterMovement;
            }
            else if (bankAccount.Movements.Count > 0 && lastKnownValue == 0)
            {
                lastKnownValue = bankAccount.Movements.OrderBy(m => m.ValueDate.Date).First().TotalBalanceAfterMovement;
            }

            return lastKnownValue;
        }

        private decimal GetLastKnownValue(BankAccount bankAccount, decimal lastKnownValue)
        {
            if (!bankAccount.Movements.Any())
            {
                var lastMovs = _myFinanceService.GetBankAccountMovements(User.GetUserId(), bankAccount.Id, 1, SortOrder.Descending);

                if (lastMovs.Any())
                {
                    var lastMov = lastMovs.First();
                    lastKnownValue = lastMov.TotalBalanceAfterMovement;
                }
            }

            return lastKnownValue;
        }
    }
}
