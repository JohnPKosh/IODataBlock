﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using HubSpot.Models.Contacts;
using Version = HubSpot.Models.Properties.PropertyVersion;

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

        #region Create / Update / Delete

        public IResponseObject<string, string> CreateContact(string value)
        {
            /* http://developers.hubspot.com/docs/methods/contacts/create_contact */
            /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/?hapikey=demo */
            
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/contact/".SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.PostStringAsync(value).Result;
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> UpdateContact(string value, int id)
        {
            /* http://developers.hubspot.com/docs/methods/contacts/update_contact */
            /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/vid/61571/profile?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/contact/vid/".AppendPathSegment(id.ToString()).AppendPathSegment("profile").SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.PostStringAsync(value).Result;
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> UpsertContact(string value, string email)
        {
            /* http://developers.hubspot.com/docs/methods/contacts/create_or_update */
            /* Example URL to POST to:  http://api.hubapi.com/contacts/v1/contact/createOrUpdate/email/test@hubspot.com/?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "http://api.hubapi.com/contacts/v1/contact/createOrUpdate/email/".AppendPathSegment(email).SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.PostStringAsync(value).Result;
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> BatchUpsertContacts(string value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject<string, string> DeleteContact(int id)
        {
            /* http://developers.hubspot.com/docs/methods/contacts/delete_contact */
            /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/61571?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/contact/vid/".AppendPathSegment(id.ToString()).SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.DeleteAsync().Result;
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        #endregion

        #region Read Contacts

        public IResponseObject<string, string> GetAllContacts(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            /* http://developers.hubspot.com/docs/methods/contacts/get_contacts */
            /* Example URL:  https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=demo */

            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/lists/all/contacts/all";

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));
                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (count.HasValue) path = path.SetQueryParam("count", count.Value);
                if (vidOffset.HasValue) path = path.SetQueryParam("vidOffset", vidOffset.Value);

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.AsStringLower());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode.AsStringLower());
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships.ToString().ToLowerInvariant());
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        public IResponseObject<string, string> GetRecentContacts(int? count = null, long? timeOffset = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            /* http://developers.hubspot.com/docs/methods/contacts/get_recently_updated_contacts */
            /* Example URL:  https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=demo */

            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent";

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));
                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (count.HasValue) path = path.SetQueryParam("count", count.Value);
                if (timeOffset.HasValue) path = path.SetQueryParam("timeOffset", timeOffset.Value);
                if (vidOffset.HasValue) path = path.SetQueryParam("vidOffset", vidOffset.Value);

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.AsStringLower());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode.AsStringLower());
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships.ToString().ToLowerInvariant());
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        public IResponseObject<string, string> GetContactById(int contactId, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            /* http://developers.hubspot.com/docs/methods/contacts/get_contact */
            /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/215913/profile?hapikey=demo */

            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/contact/vid/".AppendPathSegment(contactId.ToString()).AppendPathSegment("profile"); ;

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));
                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.AsStringLower());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode.AsStringLower());
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships.ToString().ToLowerInvariant());

                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        public IResponseObject<string, string> GetContactsByIds(IEnumerable<int> contactIds, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_vid */
            /* Example URL:  http://api.hubapi.com/contacts/v1/contact/vids/batch/?portalId=62515&vid=191254&vid=190628&hapikey=demo */

            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();
            var contactIdList = contactIds as IList<int> ?? contactIds.ToList();
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "http://api.hubapi.com/contacts/v1/contact/vids/batch/";

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));
                mergeList.AddRange(GetIdQueryParams(contactIdList));
                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.AsStringLower());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode.AsStringLower());
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships.ToString().ToLowerInvariant());
                if (includeDeleted) path = path.SetQueryParam("includeDeletes", includeDeleted.ToString().ToLowerInvariant());
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        public IResponseObject<string, string> GetContactByEmail(string email, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/contact/email/"
                    .AppendPathSegment(email)
                    .AppendPathSegment("profile");

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));
                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.AsStringLower());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode.AsStringLower());
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships.ToString().ToLowerInvariant());
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        public IResponseObject<string, string> GetContactsByEmails(IEnumerable<string> emails, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();
            var emailList = emails as IList<string> ?? emails.ToList();
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "http://api.hubapi.com/contacts/v1/contact/emails/batch/";

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));
                mergeList.AddRange(GetEmailQueryParams(emailList));
                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.AsStringLower());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode.AsStringLower());
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships.ToString().ToLowerInvariant());
                if (includeDeleted) path = path.SetQueryParam("includeDeletes", includeDeleted.ToString().ToLowerInvariant());
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        public IResponseObject<string, string> GetContactByTokenId(string contactId, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "http://api.hubapi.com/contacts/v1/contact/utk/".AppendPathSegment(contactId).AppendPathSegment("profile");

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));

                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.AsStringLower());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode.AsStringLower());
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships.ToString().ToLowerInvariant());

                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        public IResponseObject<string, string> GetContactsByTokenIds(IEnumerable<string> contactTokenIds, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();
            var contactList = contactTokenIds as IList<string> ?? contactTokenIds.ToList();
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "http://api.hubapi.com/contacts/v1/contact/utks/batch/";

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));
                mergeList.AddRange(GetUtkQueryParams(contactList));
                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (propertyMode != PropertyModeType.value_only) path = path.SetQueryParam("propertyMode", propertyMode.AsStringLower());
                if (formSubmissionMode != FormSubmissionModeType.Newest) path = path.SetQueryParam("formSubmissionMode", formSubmissionMode.AsStringLower());
                if (showListMemberships) path = path.SetQueryParam("showListMemberships", showListMemberships.ToString().ToLowerInvariant());
                if (includeDeleted) path = path.SetQueryParam("includeDeletes", includeDeleted.ToString().ToLowerInvariant());
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        public IResponseObject<string, string> GetContactsByQuery(string partialmatchNameOrEmail, int? count = 100, int? vidOffset = null, IEnumerable<string> properties = null)
        {
            if (properties == null) properties = new List<string>();
            var propertyList = properties as IList<string> ?? properties.ToList();
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v1/search/query";

                var mergeList = new List<string>();
                mergeList.AddRange(GetPropertyQueryParams(propertyList));
                if (mergeList.Any()) path += "?" + string.Join("&", mergeList);

                if (count.HasValue) path = path.SetQueryParam("count", count.Value);
                if (vidOffset.HasValue) path = path.SetQueryParam("offset", vidOffset.Value);

                path = path.SetQueryParam("q", partialmatchNameOrEmail);
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
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

        #endregion

        #region Private Utility Methods

        private IEnumerable<string> GetPropertyQueryParams(IEnumerable<string> values)
        {
            return values.Select(x => string.Format(@"property={0}", System.Web.HttpUtility.UrlEncode(x)));
        }

        private IEnumerable<string> GetEmailQueryParams(IEnumerable<string> values)
        {
            return values.Select(x => string.Format(@"email={0}", System.Web.HttpUtility.UrlEncode(x)));
        }

        private IEnumerable<string> GetUtkQueryParams(IEnumerable<string> values)
        {
            return values.Select(x => string.Format(@"utk={0}", x));
        }

        private IEnumerable<string> GetIdQueryParams(IEnumerable<int> values)
        {
            return values.Select(x => string.Format(@"vid={0}", x));
        } 

        #endregion

    }





}
