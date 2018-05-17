using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using PTZ.HomeManagement.Controllers;
using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Extentions
{
    public class RequiredPasswordChangeActionFilter : ActionFilterAttribute
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RequiredPasswordChangeActionFilter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.RouteData.Values["Controller"];
            var action = filterContext.RouteData.Values["Action"];

            string username = filterContext.HttpContext.User.Identity.Name;
            if (action.ToString() != nameof(ManageController.ChangePassword))
            {
                if (!string.IsNullOrEmpty(username))
                {
                    var user = _userManager.Users.FirstOrDefault(x => x.UserName == username);

                    if (user.RequirePasswordChange)
                    {
                        RouteValueDictionary routeValues = new RouteValueDictionary(filterContext.RouteData.Values);

                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                                { "controller", nameof(ManageController).Replace("Controller","") },
                                { "action", nameof(ManageController.ChangePassword) },
                            });
                    }
                }
            }
        }
    }
}
