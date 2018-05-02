using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            controller.ViewBag.Title = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            controller.ViewBag.GoogleMapKey = "AIzaSyBZ4OWSVpF7Mq7ryLA49FRWxo1o-ZUgzVQ";
        }
    }
}
