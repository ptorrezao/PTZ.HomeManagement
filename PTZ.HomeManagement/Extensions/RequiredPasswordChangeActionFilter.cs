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
    public class RequiredPasswordChangeActionFilterAttribute : ActionFilterAttribute
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RequiredPasswordChangeActionFilterAttribute(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["Action"];

            string username = context.HttpContext.User.Identity.Name;
            if (action.ToString() != nameof(ManageController.ChangePassword))
            {
                if (!string.IsNullOrEmpty(username))
                {
                    var user = _userManager.Users.FirstOrDefault(x => x.UserName == username);

                    if (user.RequirePasswordChange)
                    {
                        context.Result = new RedirectToRouteResult(
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
