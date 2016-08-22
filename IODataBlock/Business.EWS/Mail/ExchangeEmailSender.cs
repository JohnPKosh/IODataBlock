using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace Business.EWS.Mail
{
    public class ExchangeEmailSender : IEmailSender
    {
        #region Fields

        private readonly ExchangeService _service;

        #endregion Fields


        #region Constructors

        public ExchangeEmailSender(string serviceUrl, string userName, string password)
        {
            _service = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
            {
                Credentials = new NetworkCredential(userName, password),
                Url = new Uri(serviceUrl)
            };
        }

        #endregion Constructors

        #region Methods

        public void Send(string subject, string bodyHtml, ICollection<string> recipients)
        {
            var def = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "TempId", MapiPropertyType.String);

            var emailMessage = new EmailMessage(_service);
            emailMessage.ToRecipients.AddRange(recipients);
            emailMessage.Subject = subject;
            emailMessage.Body = bodyHtml;
            emailMessage.SetExtendedProperty(def, "test TempId");
            //emailMessage.Send();
            emailMessage.SendAndSaveCopy(WellKnownFolderName.SentItems);
        }

        public void Send(string subject, MessageBody body, IEnumerable<EmailAddress> to)
        {
            var def = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "TempId", MapiPropertyType.String);
            var emailMessage = new EmailMessage(_service);
            emailMessage.ToRecipients.AddRange(to);
            emailMessage.Subject = subject;
            emailMessage.Body = body;
            emailMessage.SetExtendedProperty(def, "test TempId");
            //emailMessage.Send();
            emailMessage.SendAndSaveCopy(WellKnownFolderName.SentItems);
        }

        public void SendAtSpecificTime(string subject, MessageBody body, IEnumerable<EmailAddress> to, DateTime sendDateTime)
        {
            var def = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "TempId", MapiPropertyType.String);

            var prDeferredSendTime = new ExtendedPropertyDefinition(16367, MapiPropertyType.SystemTime);
            var sendTime = sendDateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture);

            var emailMessage = new EmailMessage(_service);
            emailMessage.ToRecipients.AddRange(to);
            emailMessage.Subject = subject;
            emailMessage.Body = body;
            emailMessage.SetExtendedProperty(def, "test TempId");
            emailMessage.SetExtendedProperty(prDeferredSendTime, sendTime);
            //emailMessage.Send();
            emailMessage.SendAndSaveCopy(WellKnownFolderName.SentItems);
        }

        #endregion Methods
    }
}