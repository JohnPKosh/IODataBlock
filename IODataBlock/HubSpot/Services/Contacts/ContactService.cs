using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Business.Common.Extensions;
using Business.Common.GenericResponses;
using Business.Common.IO;
using Business.Common.System.States;
using Flurl;
using Flurl.Http;
using HubSpot.Models.Contacts;
using HubSpot.Models.Properties;
using HubSpot.Services.ModeTypes;
using Newtonsoft.Json;

namespace HubSpot.Services.Contacts
{
    public class ContactService : IContactService
    {
        #region Class Initialization

        public ContactService(string hapikey)
        {
            //var configMgr = new ConfigMgr();
            //_hapiKey = configMgr.GetAppSetting("hapikey");
            _hapiKey = hapikey;
            var jsonFilePath = Path.Combine(IOUtility.AppDataFolderPath, @"ContactPropertyList.json");
            var propertyManager = new PropertyManager(new ContactPropertyService(_hapiKey), new JsonFileLoader(new FileInfo(jsonFilePath)));
            ManagedProperties = propertyManager.Properties;
        }

        #endregion

        #region Fields and Properties

        private readonly string _hapiKey;

        public List<PropertyTypeModel> ManagedProperties;

        #endregion

        #region Raw API Implementation

        #region Create / Update / Delete

        public IResponseObject<string, string> Create(string value)
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

        public IResponseObject<string, string> Update(string value, int id)
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

        public IResponseObject<string, string> Upsert(string value, string email)
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

        public IResponseObject<string, string> BatchUpsert(string value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject<string, string> Delete(int id)
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

        public IResponseObject<string, string> SearchAll(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
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

        public IResponseObject<string, string> SearchRecent(int? count = null, long? timeOffset = null, int? vidOffset = null, IEnumerable<string> properties = null,
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

        public IResponseObject<string, string> GetById(int contactId, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only,
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

        public IResponseObject<string, string> SearchByIds(IEnumerable<int> contactIds, IEnumerable<string> properties = null,
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

        public IResponseObject<string, string> GetByEmail(string email, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only,
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

        public IResponseObject<string, string> SearchByEmails(IEnumerable<string> emails, IEnumerable<string> properties = null,
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

        public IResponseObject<string, string> GetByTokenId(string contactId, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only,
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

        public IResponseObject<string, string> SearchByTokenIds(IEnumerable<string> contactTokenIds, IEnumerable<string> properties = null,
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

        public IResponseObject<string, string> SearchByQuery(string partialmatchNameOrEmail, int? count = 100, int? vidOffset = null, IEnumerable<string> properties = null)
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

        #endregion

        #region Extended Implementation

        #region Read Contacts

        public ContactViewModel GetContactByEmailViewModel(string email, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            if (properties == null) properties = ManagedProperties.Select(x => x.name);

            var ro = GetByEmail(email, properties, propertyMode, formSubmissionMode, showListMemberships);
            if (ro.HasExceptions)
            {
                throw new Exception(ro.ExceptionList.Exceptions.First().Message);
            }
            var data = ro.ResponseData;
            var dto = ClassExtensions.CreateFromJson<ContactModel>(data,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                });
            return (ContactViewModel)dto;
        }

        public IEnumerable<ContactViewModel> GetAllContactViewModels(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var moreResults = true;
            var contacts = new List<ContactViewModel>();
            if (properties == null) properties = ManagedProperties.Select(x => x.name);

            while (moreResults)
            {
                var ro = SearchAll(count, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else if (string.IsNullOrWhiteSpace(ro.ResponseData))
                    moreResults = false;
                else
                {
                    var data = ro.ResponseData;
                    var dto = ClassExtensions.CreateFromJson<ContactModelList>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    moreResults = dto.has_more;
                    vidOffset = dto.vid_offset;
                    contacts.AddRange(dto.contacts.Select(c => (ContactViewModel)c));
                }
            }
            return contacts;
        }

        public IEnumerable<ContactViewModel> GetChangesContactViewModels(int? count = null, long? maxTimeOffset = null, long? minTimeOffset = null, int? maxVidOffset = null, int? minVidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var moreResults = true;
            var contacts = new List<ContactViewModel>();
            if (properties == null) properties = ManagedProperties.Select(x => x.name);
            //long? timeOffset = maxTimeOffset;
            //int? vidOffset = maxVidOffset;
            while (moreResults)
            {
                var ro = SearchRecent(count, maxTimeOffset, maxVidOffset, properties, propertyMode, formSubmissionMode, showListMemberships);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else
                {
                    var data = ro.ResponseData;
                    var dto = ClassExtensions.CreateFromJson<ContactModelList>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    maxVidOffset = dto.vid_offset;
                    maxTimeOffset = dto.time_offset;
                    var currentList = dto.contacts.Select(c => (ContactViewModel)c).ToList();

                    if (minTimeOffset.HasValue || minVidOffset.HasValue)
                    {
                        if (!minVidOffset.HasValue) minVidOffset = 0;
                        if (!minTimeOffset.HasValue) minTimeOffset = 0;
                        contacts.AddRange(currentList.Where(x=> x.addedAt >= minTimeOffset.Value && x.vid >= minVidOffset.Value));
                        if(maxTimeOffset != null && maxTimeOffset.Value < minTimeOffset.Value)break;
                        //moreResults = false;
                    }
                    else
                    {
                        contacts.AddRange(currentList);
                        //moreResults = dto.has_more;
                    }
                    moreResults = dto.has_more;
                }
            }
            return contacts;
        }

        public IEnumerable<ContactViewModel> GetRecentContactViewModels(int? count = null, long? timeOffset = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var moreResults = true;
            var contacts = new List<ContactViewModel>();
            if (properties == null) properties = ManagedProperties.Select(x => x.name);

            while (moreResults)
            {
                var ro = SearchRecent(count, timeOffset, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else
                {
                    var data = ro.ResponseData;
                    var dto = ClassExtensions.CreateFromJson<ContactModelList>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    moreResults = dto.has_more;
                    vidOffset = dto.vid_offset;
                    timeOffset = dto.time_offset;
                    contacts.AddRange(dto.contacts.Select(c => (ContactViewModel)c));
                }
            }
            return contacts;
        }

        #endregion

        #region Update Contacts

        public string GetContactUpdateString(ContactViewModel contact)
        {
            var dto = ((ContactUpdateModel)contact).ToJsonString(new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            });
            return dto;
        }

        #endregion

        #endregion

    }
}
