using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync<T>(string template, string subject, string toEmail, T model);
    }
}
