using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PTZ.HomeManagement.Extentions;
using PTZ.HomeManagement.Models;

namespace PTZ.HomeManagement.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> log)
        {
            logger = log;
        }

        public IActionResult Index()
        {
            logger.LogInformation("Hello, world!");
            return View();
        }
    }
}
