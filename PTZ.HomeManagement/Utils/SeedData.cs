using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.MyFinance.Data;
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
                foreach (var item in ApplicationDbContext.DefaultUsers)
                {
                    var userid = await EnsureUser(serviceProvider, item.Value, item.Key);

                    if (ApplicationDbContext.DefaultAdmins.Contains(item.Key))
                    {
                        await EnsureRole(serviceProvider, Roles.Administrator, userid);
                    }

                    await EnsureRole(serviceProvider, Roles.User);
                }
            }

            using (var context = new MyFinanceDbContext(
               serviceProvider.GetRequiredService<DbContextOptions<MyFinanceDbContext>>()))
            {
                foreach (var item in ApplicationDbContext.DefaultUsers)
                {
                    var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                    var user = await userManager.FindByNameAsync(item.Key);

                    if (user != null)
                    {
                        var availableCategories = new string[] { "#26C6DA", "#9C27B0", "#E53935" };
                        for (int i = 0; i < availableCategories.Length; i++)
                        {
                            var availableCategory = availableCategories[i];
                            if (!context.Categories.Any(x => x.Name == availableCategory))
                            {
                                var category = new MyFinance.Models.Category()
                                {
                                    ApplicationUser = user,
                                    Color = availableCategory,
                                    Name = "Category: " + availableCategory,
                                    Description = availableCategory,
                                };
                                context.Categories.Add(category);

                                context.Entry(category.ApplicationUser).State = EntityState.Unchanged;
                            }
                        }

                        string iban = "PT50000000000000000000001";
                        if (!context.BankAccounts.Any(x => x.IBAN == iban))
                        {
                            var bankAccount = new MyFinance.Models.BankAccount()
                            {
                                AccountType = MyFinance.AssetType.CurrentAccount,
                                Bank = MyFinance.Bank.CGD,
                                Color = "#0391CE",
                                IBAN = iban,
                                IsVisible = true,
                                Name = "Demo Account",
                                ApplicationUser = user
                            };

                            var total = 2000;
                            bankAccount.Movements.Add(new MyFinance.Models.BankAccountMovement()
                            {
                                Description = "Initial Value",
                                MovementDate = DateTime.Now.AddDays(-26),
                                ValueDate = DateTime.Now.AddDays(-26),
                                Amount = 2000,
                                TotalBalanceAfterMovement = total
                            });

                            for (int i = 25; i > 0; i--)
                            {
                                var amount = -i * 5;
                                total += amount;
                                bankAccount.Movements.Add(new MyFinance.Models.BankAccountMovement()
                                {
                                    Description = "Value " + i.ToString(),
                                    MovementDate = DateTime.Now.AddDays(-i),
                                    ValueDate = DateTime.Now.AddDays(-i),
                                    Amount = amount,
                                    TotalBalanceAfterMovement = total
                                });
                            }

                            context.Entry(bankAccount.ApplicationUser).State = EntityState.Unchanged;
                            context.BankAccounts.Add(bankAccount);
                        }

                        await context.SaveChangesAsync();

                        var categories = context.Categories.Where(x => x.ApplicationUser.Id == user.Id).Take(3).ToList();
                        var movements = context.BankAccountMovements.Where(x => x.BankAccount.ApplicationUser.Id == user.Id && x.Amount < 0).ToList();

                        foreach (var category in categories)
                        {
                            foreach (var movement in movements.Take(Convert.ToInt32(category.Id) + 4))
                            {
                                if (!context.CategoriesBankAccountMovements.Any(x => x.BankAccountMovementId == movement.Id && x.CategoryId == category.Id))
                                {
                                    context.CategoriesBankAccountMovements.Add(new MyFinance.Models.CategoryBankAccountMovement()
                                    {
                                        BankAccountMovementId = movement.Id,
                                        CategoryId = category.Id
                                    });
                                }
                            }
                        }

                        await context.SaveChangesAsync();
                    }

                }
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
                var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                var usersInRole = await userManager.GetUsersInRoleAsync(role);

                if (!usersInRole.Any(x => x.Id == userId))
                {
                    IR = await EnsureUserOnRole(serviceProvider, userId, role);
                }
            }

            return IR;
        }


        private static async Task<IdentityResult> EnsureUserOnRole(IServiceProvider serviceProvider, string userId, string role)
        {
            IdentityResult IR = null;

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                IR = await userManager.AddToRoleAsync(user, role);
            }

            return IR;
        }
    }
}
