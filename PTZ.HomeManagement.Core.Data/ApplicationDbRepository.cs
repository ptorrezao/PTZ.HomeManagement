using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.Models;

namespace PTZ.HomeManagement.Core.Data
{
    public class ApplicationDbRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationDbRepository(IServiceProvider serviceProvider)
        {
            this.context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
        }

        public ApplicationUser GetUsers(string userId = null)
        {
            return context.Users.FirstOrDefault(x => !string.IsNullOrEmpty(userId) || x.Id == userId);
        }
    }
}
