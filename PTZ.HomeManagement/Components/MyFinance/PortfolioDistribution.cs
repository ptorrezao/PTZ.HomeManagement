using Microsoft.AspNetCore.Mvc;
using PTZ.HomeManagement.Models.MyFinanceViewModels;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.MyFinance.Models;
using PTZ.HomeManagement.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace PTZ.HomeManagement.Components.MyFinance
{
    public class PortfolioDistribution : ViewComponent
    {
        private readonly IMyFinanceService _myFinanceService;

        public PortfolioDistribution(
            IMyFinanceService myFinanceService)
        {
            _myFinanceService = myFinanceService;
        }

        public async Task<IViewComponentResult> InvokeAsync(PortfolioDistributionType type)
        {
            DoughnutChartViewModel portfolioDistribution = await Task.Run(() =>
            {
                DoughnutChartViewModel vm = new DoughnutChartViewModel();
                List<BankAccount> bankAccounts = _myFinanceService.GetBankAccounts(Request.HttpContext.User.GetUserId());

                bankAccounts.Where(x => x.IsVisible).ToList().ForEach(bankAccount =>
                {
                    vm.Assets.Add(Mapper.Map<DoughnutChartItemViewModel>(bankAccount));
                });

                return vm;
            });

            return View(type.ToString(), portfolioDistribution);
        }
    }
}
