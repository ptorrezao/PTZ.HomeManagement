using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, "Ch4ng3_Th1s", "admin@hmptz.local");

                await EnsureRole(serviceProvider, Roles.Administrator, adminID);

                await EnsureRole(serviceProvider, Roles.User);

                SeedDB(context, adminID);
            }
        }


        private static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.Roles.Any())
            {
                return; // DB has been seeded
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = UserName, RequirePasswordChange = true };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string role, string userId = null)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (!string.IsNullOrEmpty(userId))
            {
                IR = await EnsureUserOnRole(serviceProvider, userId, role);
            }

            return IR;
        }


        private static async Task<IdentityResult> EnsureUserOnRole(IServiceProvider serviceProvider, string userId, string role)
        {
            IdentityResult IR = null;

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(userId);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
    }
}
