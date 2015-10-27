using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Exceptions;
using Flurl;
using Flurl.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using HubSpot.Models;
using Business.Common.Extensions;
using Business.Common.Responses;
using Version = HubSpot.Models.Version;

namespace HubSpot.Services
{
    public class ContactService : IContactService
    {
        public ContactService(string hapikey)
        {
            //var configMgr = new ConfigMgr();
            //_hapiKey = configMgr.GetAppSetting("hapikey");
            _hapiKey = hapikey;
        }

        private readonly string _hapiKey;

        public IResponseObject CreateContact(string value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject CreateContact(ContactDto value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject UpdateContact(string value, int id)
        {
            throw new NotImplementedException();
        }

        public IResponseObject UpdateContact(ContactDto value, int id)
        {
            throw new NotImplementedException();
        }

        public IResponseObject UpsertContact(string value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject UpsertContact(ContactDto value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject BatchUpsertContacts(string value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject BatchUpsertContacts(IEnumerable<ContactDto> value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject DeleteContact(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IResponseObject> GetAllContacts(int? count = null, int? vidOffset = null, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IResponseObject> GetRecentContacts(int? count = null, long? timeOffset = null, int? vidOffset = null, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactById(int? contactId, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactsByIds(IEnumerable<int> contactIds, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject<string, string> GetContactByEmail(string email, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            var ro = new ResponseObject<string, string> {RequestData = email};
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/contact/email/"
                    .AppendPathSegment(email)
                    .AppendPathSegment("profile");

                if (properties != null)
                {
                    path  += "?" + string.Join(@"&", properties.Select(x => string.Format(@"property={0}", System.Web.HttpUtility.UrlEncode(x))));
                }

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.ToString());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode);
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships);
                path = path.SetQueryParam("hapikey", _hapiKey);

                var result = path.GetStringAsync().Result;
                ro.ResponseData = result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public IResponseObject GetContactsByEmails(IEnumerable<string> email, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            //// http://api.hubapi.com/contacts/v1/contact/emails/batch/?portalId=62515&email=testingapis@hubspot.com&email=testingapisawesomeandstuff@hubspot.com&hapikey=demo

            throw new NotImplementedException();
        }

        public IResponseObject GetContactByTokenId(int? contactId, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactsByTokenIds(IEnumerable<int> contactIds, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactsByQuery(string partialmatchNameOrEmail, int? count = null, int? vidOffset = null,
            string property = null)
        {
            throw new NotImplementedException();
        }
    }
}
