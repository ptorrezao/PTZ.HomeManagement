using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Enums;
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
        private readonly IStringLocalizer<ControlController> _localizer;

        public ControlController(
            ILogger<ControlController> log,
            ICoreService coreSvc,
            IConfiguration configurationSvc,
            IStringLocalizer<ControlController> localizer)
        {
            logger = log;
            core = coreSvc;
            configuration = configurationSvc;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public string DatatablesLanguage()
        {
            var DataTableLanguage = new
            {
                sEmptyTable = _localizer["sEmptyTable"].ToString(),
                sProcessing = _localizer["sProcessing"].ToString(),
                sLengthMenu = _localizer["sLengthMenu"].ToString(),
                sZeroRecords = _localizer["sZeroRecords"].ToString(),
                sInfo = _localizer["sInfo"].ToString(),
                sInfoEmpty = _localizer["sInfoEmpty"].ToString(),
                sInfoFiltered = _localizer["sInfoFiltered"].ToString(),
                sInfoPostFix = _localizer["sInfoPostFix"].ToString(),
                sSearch = _localizer["sSearch"].ToString(),
                sUrl = "",
                oPaginate = new
                {
                    sFirst = "|«",
                    sPrevious = "«",
                    sNext = "»",
                    sLast = "»|"
                },
                oAria = new
                {
                    sSortAscending = _localizer["sSortAscending"].ToString(),
                    sSortDescending = _localizer["sSortDescending"].ToString()
                }
            };
            string output = JsonConvert.SerializeObject(DataTableLanguage);
            return output;
        }

        public IActionResult Status()
        {
            StatusViewModel vw = new StatusViewModel();
            vw.LogEntries = core.GetLastNMessages(10, LogUtils.GetLogFileName());
            vw.TotalMemory = Conversion.ConvertBytesToMegabytes(Process.GetCurrentProcess().WorkingSet64, 2);
            vw.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string envVar = Environment.GetEnvironmentVariable("DB_TYPE");
            DatabaseType dbType;
            Enum.TryParse<DatabaseType>(envVar, out dbType);

            vw.DefaultConnection = DatabaseUtils.GetConnectionString(configuration, dbType);
            return View(vw);
        }

        [AllowAnonymous]
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
