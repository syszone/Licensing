using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Licensing.Manager.Service
{
    public class EmailService : IEmailSender // this line is written by me
    {
       
        private readonly IOptions<EmailSettings> _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings;
        }
        public async Task SendEmailAsync(string emailTo, string subject, string message)
        {
            try
            {
                using (var mailClient = new GmailEmailService(_emailSettings))
                {
                    MailMessage email = new MailMessage(new MailAddress(_emailSettings.Value.Sender, _emailSettings.Value.SenderName),
                        new MailAddress(emailTo));

                    email.Subject = subject;
                    email.Body = message;
                    email.IsBodyHtml = true;
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    await mailClient.SendMailAsync(email);
                }
            }
            catch (SmtpException ex)
            {
            }

        }
    }


    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public string PasswordResetBaseUrl { get; set; }
    }
    public class GmailEmailService : SmtpClient
    {

        public GmailEmailService(IOptions<EmailSettings> emailSettings) :
            base(emailSettings.Value.MailServer, emailSettings.Value.MailPort)
        {
            this.EnableSsl = true;
            this.UseDefaultCredentials = false;
            this.Credentials = new System.Net.NetworkCredential(emailSettings.Value.Sender, emailSettings.Value.Password);
        }

    }
}
