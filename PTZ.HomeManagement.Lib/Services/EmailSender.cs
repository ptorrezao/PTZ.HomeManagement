using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
