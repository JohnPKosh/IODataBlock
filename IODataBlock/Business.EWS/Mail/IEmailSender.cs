using System.Collections.Generic;

namespace Business.EWS.Mail
{
    public interface IEmailSender
    {
        void Send(string subject, string bodyHtml, ICollection<string> recipients);
    }
}
