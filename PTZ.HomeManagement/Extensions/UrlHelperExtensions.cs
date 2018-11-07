using PTZ.HomeManagement.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static bool CheckIfIsHTML(this string text)
        {
            bool isHtml = false;
            text = text.ToLower();
            isHtml = text.Contains("<html>");
            isHtml = isHtml || text.Contains("<br>");
            isHtml = isHtml || text.Contains("<td>");
            isHtml = isHtml || text.Contains("<div>");
            return isHtml;
        }
    }
}
