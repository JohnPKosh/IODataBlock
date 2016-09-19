using Business.Common.Extensions;
using Business.Common.GenericResponses;
using Business.Common.IO;
using Business.Common.System;
using Business.Common.System.States;
using Flurl;
using Flurl.Http;
using HubSpot.Models.Companies;
using HubSpot.Models.Contacts;
using HubSpot.Models.Properties;
using HubSpot.Services.Contacts;
using HubSpot.Services.ModeTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace HubSpot.Services.Companies
{
    public class CompanyService : ICompanyService
    {
        #region Class Initialization

        public CompanyService(string hapikey)
        {
            //var configMgr = new ConfigMgr();
            //_hapiKey = configMgr.GetAppSetting("hapikey");
            _hapiKey = hapikey;
            var jsonFilePath = Path.Combine(IOUtility.AppDataFolderPath, @"CompanyPropertyList.json");
            var propertyManager = new CompanyPropertyManager(new CompanyPropertyService(_hapiKey), new JsonFileLoader(new FileInfo(jsonFilePath)));
            ManagedProperties = propertyManager.Properties;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly string _hapiKey;

        public List<PropertyTypeModel> ManagedProperties;

        #endregion Fields and Properties

        #region Raw API Implementation

        #region Create / Update / Delete

        public IResponseObject<string, string> Create(string value)
        {
            /* http://developers.hubspot.com/docs/methods/companies/create_company */
            /* Example URL to POST to:  https://api.hubapi.com/companies/v2/companies/?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/".SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.PostStringAsync(value).Result;
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> Update(string value, int id)
        {
            /* http://developers.hubspot.com/docs/methods/companies/update_company */
            /* Example URL to PUT to:  https://api.hubapi.com/companies/v2/companies/10444744?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/".AppendPathSegment(id.ToString()).SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.PutStringAsync(value).Result;
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> Delete(int id)
        {
            /* http://developers.hubspot.com/docs/methods/companies/delete_company */
            /* Example URL to DELETE to:  https://api.hubapi.com/companies/v2/companies/10444744?hapikey=demo&portalId=62515 */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/".AppendPathSegment(id.ToString()).SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.DeleteAsync().Result;
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> AddContactToCompany(int companyId, int contactId)
        {
            /* http://developers.hubspot.com/docs/methods/companies/add_contact_to_company */
            /* Example URL to PUT to:  https://api.hubapi.com/companies/v2/companies/39238082/contacts/270146?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/".AppendPathSegment(companyId.ToString()).AppendPathSegment("contacts").AppendPathSegment(contactId.ToString()).SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.PutJsonAsync(new { hapikey = _hapiKey, companyId, vid = contactId}).Result; // TODO: review if this still works?  Document says put, but looks like a GET method? Needed to modify signature because of Flurl version update.
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> RemoveContactFromCompany(int companyId, int contactId)
        {
            /* http://developers.hubspot.com/docs/methods/companies/remove_contact_from_company */
            /* Example URL to DELETE to:  https://api.hubapi.com/companies/v2/companies/39238082/contacts/270146?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/".AppendPathSegment(companyId.ToString()).AppendPathSegment("contacts").AppendPathSegment(contactId.ToString()).SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                HttpResponseMessage result = path.DeleteAsync().Result;
                result.EnsureSuccessStatusCode();
                ro.ResponseData = result.Content.ReadAsStringAsync().Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        #endregion Create / Update / Delete

        #region Read Contacts

        public IResponseObject<string, string> SearchRecentlyCreated(int? count = null, long? timeOffset = null)
        {
            /* http://developers.hubspot.com/docs/methods/companies/get_companies_created */
            /* Example URL:  https://api.hubapi.com/companies/v2/companies/recent/created?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/recent/created";
                if (count.HasValue) path = path.SetQueryParam("count", count.Value);
                if (timeOffset.HasValue) path = path.SetQueryParam("offset", timeOffset.Value);
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
                var result = path.GetStringAsync().Result;
                ro.ResponseData = result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> SearchRecentlyUpdated(int? count = null, long? timeOffset = null)
        {
            /* http://developers.hubspot.com/docs/methods/companies/get_companies_modified */
            /* Example URL:  https://api.hubapi.com/companies/v2/companies/recent/modified?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/recent/modified";
                if (count.HasValue) path = path.SetQueryParam("count", count.Value);
                if (timeOffset.HasValue) path = path.SetQueryParam("offset", timeOffset.Value);
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
                var result = path.GetStringAsync().Result;
                ro.ResponseData = result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> GetById(int companyId)
        {
            /* http://developers.hubspot.com/docs/methods/companies/get_company */
            /* Example URL:  https://api.hubapi.com/companies/v2/companies/10444744?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/".AppendPathSegment(companyId.ToString()).SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                var result = path.GetStringAsync().Result;
                ro.ResponseData = result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> GetByDomain(string domain)
        {
            /* http://developers.hubspot.com/docs/methods/companies/get_companies_by_domain */
            /* Example URL:  https://api.hubapi.com/companies/v2/companies/domain/example.com?hapikey=demo */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/domain/".AppendPathSegment(domain).SetQueryParam("hapikey", _hapiKey);
                ro.RequestData = path;
                var result = path.GetStringAsync().Result;
                ro.ResponseData = result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> SearchContacts(int companyId, int? count = null, int? vidOffset = null)
        {
            /* http://developers.hubspot.com/docs/methods/companies/get_company_contacts */
            /* Example URL:  https://api.hubapi.com/companies/v2/companies/10444744/contacts?hapikey=demo&portalId=62515 */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/".AppendPathSegment(companyId.ToString()).AppendPathSegment("contacts");
                if (count.HasValue) path = path.SetQueryParam("count", count.Value);
                if (vidOffset.HasValue) path = path.SetQueryParam("vidOffset", vidOffset.Value);
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
                var result = path.GetStringAsync().Result;
                ro.ResponseData = result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, string> SearchContactIds(int companyId, int? count = null, int? vidOffset = null)
        {
            /* http://developers.hubspot.com/docs/methods/companies/get_company_contacts_by_id */
            /* Example URL:  https://api.hubapi.com/companies/v2/companies/10444744/vids?hapikey=demo&portalId=62515 */

            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/companies/".AppendPathSegment(companyId.ToString()).AppendPathSegment("vids");
                if (count.HasValue) path = path.SetQueryParam("count", count.Value);
                if (vidOffset.HasValue) path = path.SetQueryParam("vidOffset", vidOffset.Value);
                path = path.SetQueryParam("hapikey", _hapiKey);

                ro.RequestData = path;
                var result = path.GetStringAsync().Result;
                ro.ResponseData = result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        #endregion Read Contacts

        #region Private Utility Methods

        //private IEnumerable<string> GetPropertyQueryParams(IEnumerable<string> values)
        //{
        //    return values.Select(x => string.Format(@"property={0}", System.Web.HttpUtility.UrlEncode(x)));
        //}

        //private IEnumerable<string> GetEmailQueryParams(IEnumerable<string> values)
        //{
        //    return values.Select(x => string.Format(@"email={0}", System.Web.HttpUtility.UrlEncode(x)));
        //}

        //private IEnumerable<string> GetUtkQueryParams(IEnumerable<string> values)
        //{
        //    return values.Select(x => string.Format(@"utk={0}", x));
        //}

        //private IEnumerable<string> GetIdQueryParams(IEnumerable<int> values)
        //{
        //    return values.Select(x => string.Format(@"vid={0}", x));
        //}

        #endregion Private Utility Methods

        #endregion Raw API Implementation

        #region Extended Implementation

        #region Read Contacts

        public CompanyViewModel GetByIdViewModel(int companyId)
        {
            var ro = GetById(companyId);
            if (ro.HasExceptions)
            {
                throw new Exception(ro.ExceptionList.Exceptions.First().Message);
            }
            var data = ro.ResponseData;
            var dto = ClassExtensions.CreateFromJson<CompanyModel>(data,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                });
            return (CompanyViewModel)dto;
        }

        public IEnumerable<CompanyViewModel> GetByDomainViewModel(string domain)
        {
            var ro = GetByDomain(domain);
            if (ro.HasExceptions)
            {
                throw new Exception(ro.ExceptionList.Exceptions.First().Message);
            }
            var data = ro.ResponseData;
            var dto = ClassExtensions.CreateFromJson<CompanyModel[]>(data,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                });
            return dto.Select(x => (CompanyViewModel)x);
        }

        public IEnumerable<dynamic> GetAllContactsDynamic(int companyId, int? count = null, int? vidOffset = null)
        {
            var moreResults = true;
            var contacts = new List<dynamic>();

            while (moreResults)
            {
                var ro = SearchContacts(companyId, count, vidOffset);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else if (string.IsNullOrWhiteSpace(ro.ResponseData))
                    moreResults = false;
                else
                {
                    var data = ro.ResponseData;
                    dynamic dto = ClassExtensions.CreateFromJson<ExpandoObject>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    moreResults = dto.hasMore;
                    vidOffset = dto.vidOffset as int?;
                    contacts.AddRange(dto.contacts);
                }
            }
            return contacts;
        }

        public IEnumerable<dynamic> GetAllContactIdsDynamic(int companyId, int? count = null, int? vidOffset = null)
        {
            var moreResults = true;
            var contacts = new List<dynamic>();

            while (moreResults)
            {
                var ro = SearchContactIds(companyId, count, vidOffset);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else if (string.IsNullOrWhiteSpace(ro.ResponseData))
                    moreResults = false;
                else
                {
                    var data = ro.ResponseData;
                    dynamic dto = ClassExtensions.CreateFromJson<ExpandoObject>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    moreResults = dto.hasMore;
                    vidOffset = dto.vidOffset as int?;
                    contacts.AddRange(dto.vids);
                }
            }
            return contacts;
        }

        public IEnumerable<ContactViewModel> GetAllContactViewModels(int companyId, int? count = null, int? vidOffset = null)
        {
            var moreResults = true;
            var contacts = new List<int>();

            while (moreResults)
            {
                var ro = SearchContactIds(companyId, count, vidOffset);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else if (string.IsNullOrWhiteSpace(ro.ResponseData))
                    moreResults = false;
                else
                {
                    var data = ro.ResponseData;
                    dynamic dto = ClassExtensions.CreateFromJson<CompanyContactIdViewModel>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    moreResults = dto.hasMore;
                    vidOffset = dto.vidOffset as int?;
                    contacts.AddRange(dto.vids);
                }
            }
            return contacts.Select(x => GetContact((int)x));
        }

        private ContactViewModel GetContact(int contactId)
        {
            var service = new ContactService(_hapiKey);
            var ro = service.GetById(contactId, propertyMode: PropertyModeType.value_and_history, showListMemberships: true);
            if (!ro.HasExceptions)
            {
                return ClassExtensions.CreateFromJson<ContactModel>(ro.ResponseData);
            }
            else
            {
                throw new Exception(ro.ExceptionList.Exceptions.First().Message);
            }
        }

        public IEnumerable<CompanyViewModel> GetChangesCompanyViewModels(int? count = null, long? maxTimeOffset = null, long? minTimeOffset = null)
        {
            var moreResults = true;
            var contacts = new List<CompanyViewModel>();
            //if (properties == null) properties = ManagedProperties.Select(x => x.name);
            //long? timeOffset = maxTimeOffset;
            //int? vidOffset = maxVidOffset;
            while (moreResults)
            {
                var ro = SearchRecentlyCreated(count, maxTimeOffset);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else
                {
                    var data = ro.ResponseData;
                    var dto = ClassExtensions.CreateFromJson<CompanyModelList>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    //maxVidOffset = dto.vid_offset;
                    //maxTimeOffset = dto.offset;

                    var currentList = dto.results.Select(c => (CompanyViewModel)c).ToList();

                    if (minTimeOffset.HasValue)
                    {
                        contacts.AddRange(currentList.Where(x => long.Parse(x.Properties.First(y => y.Key == "createdate").Value) >= minTimeOffset.Value));
                        if (maxTimeOffset != null && maxTimeOffset.Value < minTimeOffset.Value) break;
                        //moreResults = false;
                    }
                    else
                    {
                        contacts.AddRange(currentList);
                        //moreResults = dto.has_more;
                    }
                    var min = (DateTime?)new UnixMsTimestamp(contacts.Min(x => x.Properties.First(y => y.Key == "createdate").Value));
                    var max = (DateTime?)new UnixMsTimestamp(contacts.Max(x => x.Properties.First(y => y.Key == "createdate").Value));
                    maxTimeOffset = new UnixMsTimestamp(contacts.Min(x => x.Properties.First(y => y.Key == "createdate").Value));
                    //moreResults = dto.has_more;
                }
            }
            return contacts;
        }

        public IEnumerable<CompanyViewModel> GetRecentCompanyViewModels(int? count = null, long? timeOffset = null)
        {
            var moreResults = true;
            var companies = new List<CompanyViewModel>();
            while (moreResults)
            {
                var ro = SearchRecentlyCreated(count, timeOffset);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else
                {
                    var data = ro.ResponseData;
                    var dto = ClassExtensions.CreateFromJson<CompanyModelList>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    moreResults = dto.has_more;
                    timeOffset = dto.offset;
                    companies.AddRange(dto.results.Select(c => (CompanyViewModel)c));
                }
            }
            return companies;
        }

        public IEnumerable<CompanyViewModel> GetAllCompanyViewModels(int? count = null, long? offset = null)
        {
            var moreResults = true;
            var companies = new List<CompanyViewModel>();
            while (moreResults)
            {
                var ro = SearchRecentlyCreated(count, offset);
                if (ro.HasExceptions)
                {
                    throw new Exception(ro.ExceptionList.Exceptions.First().Message);
                }
                else
                {
                    var data = ro.ResponseData;
                    var dto = ClassExtensions.CreateFromJson<CompanyModelList>(data,
                        new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        });
                    offset = dto.offset;
                    companies.AddRange(dto.results.Select(c => (CompanyViewModel)c));
                    moreResults = dto.has_more;
                }
            }
            return companies;
        }

        //public IEnumerable<ContactViewModel> GetAllContactViewModels(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
        //    PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
        //    bool showListMemberships = false)
        //{
        //    var moreResults = true;
        //    var contacts = new List<ContactViewModel>();
        //    if (properties == null) properties = ManagedProperties.Select(x => x.name);

        //    while (moreResults)
        //    {
        //        var ro = SearchAll(count, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships);
        //        if (ro.HasExceptions)
        //        {
        //            throw new Exception(ro.ExceptionList.Exceptions.First().Message);
        //        }
        //        else if (string.IsNullOrWhiteSpace(ro.ResponseData))
        //            moreResults = false;
        //        else
        //        {
        //            var data = ro.ResponseData;
        //            var dto = ClassExtensions.CreateFromJson<ContactModelList>(data,
        //                new JsonSerializerSettings()
        //                {
        //                    NullValueHandling = NullValueHandling.Include,
        //                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
        //                });
        //            moreResults = dto.has_more;
        //            vidOffset = dto.vid_offset;
        //            contacts.AddRange(dto.contacts.Select(c => (ContactViewModel)c));
        //        }
        //    }
        //    return contacts;
        //}

        //public IEnumerable<ContactViewModel> GetChangesContactViewModels(int? count = null, long? maxTimeOffset = null, long? minTimeOffset = null, int? maxVidOffset = null, int? minVidOffset = null, IEnumerable<string> properties = null,
        //    PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
        //    bool showListMemberships = false)
        //{
        //    var moreResults = true;
        //    var contacts = new List<ContactViewModel>();
        //    if (properties == null) properties = ManagedProperties.Select(x => x.name);
        //    //long? timeOffset = maxTimeOffset;
        //    //int? vidOffset = maxVidOffset;
        //    while (moreResults)
        //    {
        //        var ro = SearchRecent(count, maxTimeOffset, maxVidOffset, properties, propertyMode, formSubmissionMode, showListMemberships);
        //        if (ro.HasExceptions)
        //        {
        //            throw new Exception(ro.ExceptionList.Exceptions.First().Message);
        //        }
        //        else
        //        {
        //            var data = ro.ResponseData;
        //            var dto = ClassExtensions.CreateFromJson<ContactModelList>(data,
        //                new JsonSerializerSettings()
        //                {
        //                    NullValueHandling = NullValueHandling.Include,
        //                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
        //                });
        //            maxVidOffset = dto.vid_offset;
        //            maxTimeOffset = dto.time_offset;
        //            var currentList = dto.contacts.Select(c => (ContactViewModel)c).ToList();

        //            if (minTimeOffset.HasValue || minVidOffset.HasValue)
        //            {
        //                if (!minVidOffset.HasValue) minVidOffset = 0;
        //                if (!minTimeOffset.HasValue) minTimeOffset = 0;
        //                contacts.AddRange(currentList.Where(x=> x.addedAt >= minTimeOffset.Value && x.vid >= minVidOffset.Value));
        //                if(maxTimeOffset != null && maxTimeOffset.Value < minTimeOffset.Value)break;
        //                //moreResults = false;
        //            }
        //            else
        //            {
        //                contacts.AddRange(currentList);
        //                //moreResults = dto.has_more;
        //            }
        //            moreResults = dto.has_more;
        //        }
        //    }
        //    return contacts;
        //}

        //public IEnumerable<ContactViewModel> GetRecentContactViewModels(int? count = null, long? timeOffset = null, int? vidOffset = null, IEnumerable<string> properties = null,
        //    PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
        //    bool showListMemberships = false)
        //{
        //    var moreResults = true;
        //    var contacts = new List<ContactViewModel>();
        //    if (properties == null) properties = ManagedProperties.Select(x => x.name);

        //    while (moreResults)
        //    {
        //        var ro = SearchRecent(count, timeOffset, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships);
        //        if (ro.HasExceptions)
        //        {
        //            throw new Exception(ro.ExceptionList.Exceptions.First().Message);
        //        }
        //        else
        //        {
        //            var data = ro.ResponseData;
        //            var dto = ClassExtensions.CreateFromJson<ContactModelList>(data,
        //                new JsonSerializerSettings()
        //                {
        //                    NullValueHandling = NullValueHandling.Include,
        //                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
        //                });
        //            moreResults = dto.has_more;
        //            vidOffset = dto.vid_offset;
        //            timeOffset = dto.time_offset;
        //            contacts.AddRange(dto.contacts.Select(c => (ContactViewModel)c));
        //        }
        //    }
        //    return contacts;
        //}

        #endregion Read Contacts

        #region Update Contacts

        public string GetContactUpdateString(CompanyViewModel contact)
        {
            var dto = ((CompanyUpdateModel)contact).ToJsonString(new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            });
            return dto;
        }

        #endregion Update Contacts

        #endregion Extended Implementation
    }
}