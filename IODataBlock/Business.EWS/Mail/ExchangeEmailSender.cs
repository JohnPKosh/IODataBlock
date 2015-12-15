using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace Business.EWS.Mail
{
    public class ExchangeEmailSender : IEmailSender
    {
        #region Fields

        private readonly ExchangeService _service;

        #endregion

        #region Credential

        readonly string _serviceUrl;
        readonly string _userName;
        readonly string _password;

        #endregion

        #region Constructors

        public ExchangeEmailSender(string serviceUrl, string userName, string password)
        {
            _serviceUrl = serviceUrl;
            _userName = userName;
            _password = password;

            _service = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
            {
                Credentials = new NetworkCredential(_userName, _password),
                Url = new Uri(_serviceUrl)
            };
        }

        #endregion

        #region Methods

        public void Send(string subject, string bodyHtml, ICollection<string> recipients)
        {
            var emailMessage = new EmailMessage(_service);
            emailMessage.ToRecipients.AddRange(recipients);
            emailMessage.Subject = subject;
            emailMessage.Body = bodyHtml;
            emailMessage.Send();
        }

        public void Send(string subject, MessageBody body, IEnumerable<EmailAddress> to)
        {
            var emailMessage = new EmailMessage(_service);
            emailMessage.ToRecipients.AddRange(to);
            emailMessage.Subject = subject;
            emailMessage.Body = body;
            emailMessage.Send();
        }

        #endregion
    }

}
