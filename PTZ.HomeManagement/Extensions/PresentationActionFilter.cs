using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Extentions
{
    public class PresentationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //  get the view bag
            var controller = context.Controller as Controller;

            // set the viewbag values
            controller.ViewBag.AppName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            controller.ViewBag.Title = context.RouteData.Values["Action"];
            controller.ViewBag.GoogleMapKey = "AIzaSyBZ4OWSVpF7Mq7ryLA49FRWxo1o-ZUgzVQ";

            controller.ViewBag.Version = typeof(Startup).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            controller.ViewBag.CurrentAction = context.RouteData.Values["Action"];
            controller.ViewBag.CurrentController = context.RouteData.Values["Controller"];
            controller.ViewBag.CurrentArea = context.RouteData.Values["Area"];
        }
    }
}
