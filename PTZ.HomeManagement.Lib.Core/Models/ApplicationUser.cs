using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;

namespace PTZ.HomeManagement.Models
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        public bool RequirePasswordChange { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}
