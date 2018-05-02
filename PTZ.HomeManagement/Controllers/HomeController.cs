using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PTZ.HomeManagement.Extentions;
using PTZ.HomeManagement.Models;

namespace PTZ.HomeManagement.Controllers
{
    public class HomeController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
