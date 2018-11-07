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

        public string FullName
        {
            get
            {
                string fullName = null;

                fullName = !string.IsNullOrEmpty(this.FirstName) ? this.FirstName + " " : null;
                fullName = !string.IsNullOrEmpty(this.LastName) ? fullName + this.LastName : null;
                return fullName == null ? null : fullName.Trim();
            }
        }
        [NotMapped]
        public bool HasGmailAppPassword
        {
            get
            {
                return !string.IsNullOrEmpty(this.Email)  && 
                    this.Email.Contains("@gmail.com") && 
                    !string.IsNullOrEmpty(this.AppPassword);
            }
        }

        public string AppPassword { get; set; }

    }
}
