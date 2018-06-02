using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "NewPassword_ErroMessage", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("NewPassword", ErrorMessage = "ConfirmPassword_Error")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
