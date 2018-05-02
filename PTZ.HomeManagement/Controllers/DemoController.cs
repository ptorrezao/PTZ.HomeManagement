using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PTZ.HomeManagement.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Icons()
        {
            return View();
        }

        public IActionResult Maps()
        {
            return View();
        }

        public IActionResult Notifications()
        {
            return View();
        }

        public IActionResult Table()
        {
            return View();
        }

        public IActionResult Typography()
        {
            return View();
        }

        public IActionResult User()
        {
            return View();
        }
    }
}