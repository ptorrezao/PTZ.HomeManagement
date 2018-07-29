using Microsoft.AspNetCore.Identity;
using PTZ.HomeManagement.Models.MyFinance;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool RequirePasswordChange { get; set; }
    }
}
