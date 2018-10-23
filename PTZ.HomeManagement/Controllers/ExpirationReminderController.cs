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

        public IActionResult ListReminderCategories()
        {
            List<ReminderCategory> reminders = _expirationReminderService.GetReminderCategories(User.GetUserId());
            return View(Mapper.Map<ReminderCategoryListViewModel>(reminders));
        }

        public IActionResult AddOrEditReminder(int? id)
        {
            Reminder reminder = id.HasValue ? _expirationReminderService.GetReminder(User.GetUserId(), id.Value) : _expirationReminderService.GetReminderDefault(User.GetUserId());
            ReminderViewModel viewModel = Mapper.Map<ReminderViewModel>(reminder);

            List<ReminderCategory> categories = _expirationReminderService.GetReminderCategories(User.GetUserId());
            viewModel.SetAvailableCategories(Mapper.Map<List<ReminderCategory>, List<ReminderCategoryViewModel>>(categories));

            return View(viewModel);
        }

        public IActionResult AddOrEditReminderCategory(int? id)
        {
            ReminderCategory reminder = id.HasValue ? _expirationReminderService.GetReminderCategory(User.GetUserId(), id.Value) : _expirationReminderService.GetReminderCategoryDefault(User.GetUserId());
            return View(Mapper.Map<ReminderCategoryViewModel>(reminder));
        }

        [HttpPost]
        public IActionResult AddOrEditReminder(ReminderViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                _expirationReminderService.SaveReminder(User.GetUserId(), Mapper.Map<Reminder>(rvm));
                _expirationReminderService.SetCategoriesToReminder(User.GetUserId(), rvm.Id, rvm.SelectedCategories);
                return RedirectToAction(nameof(ListReminders));
            }

            return View(rvm);
        }

        [HttpPost]
        public IActionResult AddOrEditReminderCategory(ReminderCategoryViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                _expirationReminderService.SaveReminderCategory(User.GetUserId(), Mapper.Map<ReminderCategory>(rvm));

                return RedirectToAction(nameof(ListReminderCategories));
            }

            return View(rvm);
        }

        public IActionResult DeleteReminder(int id)
        {
            Reminder reminder = _expirationReminderService.GetReminder(User.GetUserId(), id);
            return View(Mapper.Map<ReminderViewModel>(reminder));
        }

        public IActionResult DeleteReminderCategory(int id)
        {
            ReminderCategory reminder = _expirationReminderService.GetReminderCategory(User.GetUserId(), id);
            return View(Mapper.Map<ReminderCategoryViewModel>(reminder));
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

        [HttpPost]
        public IActionResult DeleteReminderCategory(ReminderCategoryViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                _expirationReminderService.DeleteReminderCategory(User.GetUserId(), Mapper.Map<ReminderCategory>(rvm));

                return RedirectToAction(nameof(ListReminderCategories));
            }

            return View(rvm);
        }
    }
}