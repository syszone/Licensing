using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Interface
{
    public interface IEmailSend
    {
        Task SendEmailAsync(string emailTo, string subject, string body, string attachments);
    }
}
