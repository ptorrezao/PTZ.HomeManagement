using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTZ.HomeManagement.ExpirationReminder.Services;
using PTZ.HomeManagement.Models.ExpirationReminderModels;
using PTZ.HomeManagement.ExpirationReminder.Core;
using PTZ.HomeManagement.Utils;
using AutoMapper;

namespace PTZ.HomeManagement.Controllers
{
    public class ExpirationReminderController : Controller
    {
        private readonly IExpirationReminderService _expirationReminderService;

        public ExpirationReminderController(IExpirationReminderService expirationReminderService)
        {
            this._expirationReminderService = expirationReminderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListReminders()
        {
            List<Reminder> reminders = _expirationReminderService.GetReminders(User.GetUserId());
            return View(Mapper.Map<ReminderListViewModel>(reminders));
        }

        public IActionResult AddOrEditReminder(int? id)
        {
            Reminder reminder = id.HasValue ? _expirationReminderService.GetReminder(User.GetUserId(), id.Value) : _expirationReminderService.GetReminderDefault(User.GetUserId());
            return View(Mapper.Map<ReminderViewModel>(reminder));
        }

        [HttpPost]
        public IActionResult AddOrEditReminder(ReminderViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                _expirationReminderService.SaveReminder(User.GetUserId(), Mapper.Map<Reminder>(rvm));

                return RedirectToAction(nameof(ListReminders));
            }

            return View(rvm);
        }


        public IActionResult DeleteReminder(int id)
        {
            Reminder reminder = _expirationReminderService.GetReminder(User.GetUserId(), id);
            return View(Mapper.Map<ReminderViewModel>(reminder));
        }

        [HttpPost]
        public IActionResult DeleteReminder(ReminderViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                _expirationReminderService.DeleteReminder(User.GetUserId(), Mapper.Map<Reminder>(rvm));

                return RedirectToAction(nameof(ListReminders));
            }

            return View(rvm);
        }
    }
}