using AEFWeb.Core.Services;
using AEFWeb.Implementation.EmailSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Services
{
    public class EmailService : IEmailService
    {
        public EmailSetting _emailSettings { get; }


        public EmailService(IOptions<EmailSetting> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string email, string subject, string message)
        {
            string toEmail = string.IsNullOrEmpty(email)
                             ? _emailSettings.ToEmail
                             : email;
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(_emailSettings.UsernameEmail, "AEF")
            };

            mail.To.Add(new MailAddress(toEmail));
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
            {
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
