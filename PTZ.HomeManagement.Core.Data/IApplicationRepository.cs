using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PTZ.HomeManagement.Core.Data
{
    public interface IApplicationRepository
    {
        IQueryable<ApplicationUser> GetUsers(string userId = null);
        ApplicationUser GetUser(string userId);
        void SaveUser(ApplicationUser user);
        bool OnlyDefaultUserIsAvailable();
        string GetConfiguration(string configName);
    }
}
