using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mime;
using Licensing.Manager.Interface;

namespace Licensing.Manager.Service
{
    public class EmailServiceAttachment   : IEmailSend
    {
        private readonly IOptions<EmailSettings> _emailSettings;
        public EmailServiceAttachment(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(string emailTo, string subject,string body,string attachments)
        {
            try
            {
                using (var mailClient = new GmailEmailService(_emailSettings))
                {
                    MailMessage email = new MailMessage(new MailAddress(_emailSettings.Value.Sender, _emailSettings.Value.SenderName),
                        new MailAddress(emailTo));

                    email.Subject = subject;
                    email.Body = body;
                    email.IsBodyHtml = true;
                    Attachment data = new Attachment(attachments, MediaTypeNames.Application.Octet);                    
                    email.Attachments.Add(data);
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

  
}
