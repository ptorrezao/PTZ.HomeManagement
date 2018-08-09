using PTZ.HomeManagement.Models;
using System;

namespace PTZ.HomeManagement.Core.Data
{
    public interface IApplicationRepository
    {
        ApplicationUser GetUsers(string userId = null);
    }
}
