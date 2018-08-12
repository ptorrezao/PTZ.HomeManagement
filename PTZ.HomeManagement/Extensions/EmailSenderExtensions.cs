using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync("EmailConfirm", "PTZ.HomeAssistant - Please Confirm your email", "pedro.torrezao@gmail.com", new { Email = email, Link = HtmlEncoder.Default.Encode(link), Subject = "PTZ.HomeAssistant - Please Confirm your email" });
        }

        public static Task SendEmailResetPasswordAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync("PasswordRecovery", "PTZ.HomeAssistant - Recover Email", "pedro.torrezao@gmail.com", new { Email = email, Link = link, Subject = "PTZ.HomeAssistant - Recover Email" });
        }
    }
}
