using GravatarSharp.Core;
using Microsoft.AspNetCore.Identity;
using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Utils
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this IPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return (principal as ClaimsPrincipal).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetUserFullName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        public static string GetUserGravatar(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.UserData)?.Value;

        }

        public static async Task UpdataAllClaims(ApplicationUser user, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, bool forceRefresh = false)
        {
            var shouldUpdate = false;
            var claims = await userManager.GetClaimsAsync(user);

            shouldUpdate = await UpdateClaims(user, userManager, claims, ClaimTypes.GivenName, user.FullName ?? user.UserName);
            shouldUpdate = await UpdateClaims(user, userManager, claims, ClaimTypes.UserData, GravatarController.GetImageUrl(user.Email ?? user.UserName)) && shouldUpdate;

            if (shouldUpdate || forceRefresh)
            {
                await signInManager.RefreshSignInAsync(user);
            }
        }

        private static async Task<bool> UpdateClaims(ApplicationUser user, UserManager<ApplicationUser> userManager, System.Collections.Generic.IList<Claim> claims, string type, string value)
        {
            var shouldUpdate = false;
            if (claims.Any(x => x.Type == type && x.Value != value) ||
                !claims.Any(x => x.Type == type))
            {
                await userManager.RemoveClaimsAsync(user, claims.Where(x => x.Type == type));
                await userManager.AddClaimAsync(user, new Claim(type, value));

                shouldUpdate = true;
            }

            return shouldUpdate;
        }
    }
}
