using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.Services;
using PTZ.HomeManagement.Utils;

namespace PTZ.HomeManagement.Controllers
{
    public class ControlController : Controller
    {
        private readonly ILogger<ControlController> logger;
        private readonly ICoreService core;
        private readonly IConfiguration configuration;

        public ControlController(
            ILogger<ControlController> log,
            ICoreService coreSvc,
            IConfiguration configurationSvc)
        {
            logger = log;
            core = coreSvc;
            configuration = configurationSvc;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Status()
        {
            StatusViewModel vw = new StatusViewModel();
            vw.LogEntries = core.GetLastNMessages(10, LogUtils.GetLogFileName());
            vw.TotalMemory = Conversion.ConvertBytesToMegabytes(Process.GetCurrentProcess().WorkingSet64, 2);
            vw.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            vw.DefaultConnection = ApplicationDbContext.GetConnectionString(configuration);
            return View(vw);
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
