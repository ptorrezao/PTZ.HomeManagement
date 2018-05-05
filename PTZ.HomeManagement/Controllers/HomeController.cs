using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            return View();
        }

        public IActionResult Error(int? statusCode = null)
        {
            IExceptionHandlerPathFeature exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            string message = "", detail = "";

            if (statusCode.HasValue && statusCode.Value == 404)
            {
                message = "Uh oh! Looks like you’re lost...";
            }
            else if (exceptionFeature != null)
            {
                Exception exceptionThatOccurred = exceptionFeature.Error;

                message = "Looks like we're having some server issues.";
                detail = exceptionThatOccurred.Message;

                logger.LogError(exceptionThatOccurred, exceptionThatOccurred.Message);
            }

            ViewBag.Message = message;
            ViewBag.MessageDetail = detail;
            ViewBag.StatusCode = statusCode ?? 500;
            return View();
        }
    }
}
