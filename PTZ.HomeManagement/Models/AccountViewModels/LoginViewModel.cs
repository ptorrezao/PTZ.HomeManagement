using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email_Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password_Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "RadioButton_Remember_Me")]
        public bool RememberMe { get; set; }
        public string Message { get; set; }
    }
}
