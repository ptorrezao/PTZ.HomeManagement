using Microsoft.AspNetCore.Mvc;
using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using PTZ.HomeManagement.ExpirationReminder.Services;
using PTZ.HomeManagement.Models.ExpirationReminderModels;
using PTZ.HomeManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Components.ExpirationReminder
{
    public class ReminderOverview : ViewComponent
    {
        private readonly IExpirationReminderService service;

        public ReminderOverview(
            IExpirationReminderService _service)
        {
            service = _service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ReminderOverviewListItem> viewModel = await Task.Run(() =>
            {
                List<ReminderOverviewListItem> vm = new List<ReminderOverviewListItem>();
                List<ReminderStateType> reminderTypes = new List<ReminderStateType>() { ReminderStateType.NonExpired, ReminderStateType.Expiring, ReminderStateType.Expired };

                foreach (ReminderStateType item in reminderTypes)
                {
                    vm.Add(new ReminderOverviewListItem()
                    {
                        Quantity = service.GetRemindersByType(User.GetUserId(), new List<ReminderStateType>() { item }).Count,
                        Type = item
                    });
                }

                return vm;
            });

            return View(viewModel);
        }
    }
}
