using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "IsEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        public bool HasGmailAppPassword
        {
            get
            {
                return !string.IsNullOrEmpty(this.Email) &&
                    this.Email.Contains("@gmail.com");
            }
        }

        [Display(Name = "App Password")]
        public string AppPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
