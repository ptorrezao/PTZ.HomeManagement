using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Mailgun;
using FluentEmail.Razor;
using Microsoft.Extensions.Options;
using PTZ.HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly Email emailSender;

        public EmailSender(IOptions<EmailSettings> emailOptions)
        {
            _emailSettings = emailOptions.Value;

            var sender = new MailgunSender(
                _emailSettings.Domain, // Mailgun Domain
                _emailSettings.ApiKey// Mailgun API Key
            );

            var razorRenderer = new RazorRenderer();


            emailSender = new Email(razorRenderer, sender);
        }

        private void CheckSettings()
        {
            if (string.IsNullOrEmpty(_emailSettings.From))
            {
                throw new EmailSenderException($"Parameter _emailSettings.From can't be null");
            }

            if (string.IsNullOrEmpty(_emailSettings.Domain))
            {
                throw new EmailSenderException($"Parameter _emailSettings.Domain can't be null");
            }

            if (string.IsNullOrEmpty(_emailSettings.ApiKey))
            {
                throw new EmailSenderException($"Parameter _emailSettings.ApiKey can't be null");
            }
        }

        public async Task SendEmailAsync<T>(string template, string subject, string toEmail, T model)
        {
            CheckSettings();

            var email = emailSender
                .SetFrom(_emailSettings.From)
                .To(toEmail)
                .Subject(subject)
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Views/Shared/EmailTemplates/{template}.cshtml", model);

            await email.SendAsync();
        }

        public SendResponse SendEmail<T>(string template, string subject, string toEmail, T model, string path = "/wwwroot/Views/Shared/EmailTemplates/")
        {
            CheckSettings();

            var email = emailSender
                .SetFrom(_emailSettings.From)
                .To(toEmail)
                .Subject(subject)
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}{path}{template}.cshtml", model);

            return email.Send();
        }
    }
}
