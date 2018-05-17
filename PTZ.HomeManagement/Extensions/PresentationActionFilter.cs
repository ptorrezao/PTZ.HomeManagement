using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Extentions
{
    public class PresentationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //  get the view bag
            var controller = filterContext.Controller as Controller;

            // set the viewbag values
            controller.ViewBag.AppName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            controller.ViewBag.Title = filterContext.RouteData.Values["Action"];
            controller.ViewBag.GoogleMapKey = "AIzaSyBZ4OWSVpF7Mq7ryLA49FRWxo1o-ZUgzVQ";

            controller.ViewBag.CurrentAction = filterContext.RouteData.Values["Action"];
            controller.ViewBag.CurrentController = filterContext.RouteData.Values["Controller"];
            controller.ViewBag.CurrentArea = filterContext.RouteData.Values["Area"];
        }
    }
}
