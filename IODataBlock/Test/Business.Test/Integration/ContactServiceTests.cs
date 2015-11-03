using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Extensions;
using Business.Common.System;
using HubSpot.Models;
using HubSpot.Models.Contacts;
using HubSpot.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Business.Test.Integration
{
    [TestClass]
    public class ContactServiceTests
    {

        public ContactServiceTests()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
        }

        private readonly string _hapiKey;

        [TestMethod]
        public void GetAllContactsTest()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            var ro = service.GetAllContacts(propertyMode: PropertyModeType.value_and_history);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactModelList>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
                Assert.IsNotNull(dto);

                ContactViewModel update = dto.contacts.First(x=>x.vid == 315);
                Assert.IsNotNull(update);
              

                update.WriteJsonToFilePath(@"c:\junk\ContactUpdate.json");
            }
        }

        [TestMethod]
        public void GetAllContactsDynamicTest()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            var ro = service.GetAllContacts(propertyMode: PropertyModeType.value_and_history);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ExpandoObject>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
                Assert.IsNotNull(dto);

                var jdata = JObject.Parse(data);
                Assert.IsNotNull(jdata);
                File.WriteAllText(@"C:\junk\rawgetallcontacts.json", data);
            }
        }

        [TestMethod]
        public void GetAllContactsPagingTest()
        {
            var service = new ContactService(_hapiKey);
            //var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };

            int? lastId = null;
            var moreResults = true;
            var contacts = new List<ContactViewModel>();

            while (moreResults)
            {
                var ro = service.GetAllContacts(100, lastId, propertyMode: PropertyModeType.value_and_history, formSubmissionMode:FormSubmissionModeType.All, showListMemberships:true);
                if (ro.HasExceptions)
                {
                    Assert.Fail();
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
                    lastId = dto.vid_offset;
                    contacts.AddRange(dto.contacts.Select(c => (ContactViewModel)c));
                }
            }

            foreach (var c in contacts)
            {
                var versioned = c.Properties.Where(x => x.Versions != null);
                if (versioned.Any(x=> x.Versions.Count > 1))
                {
                    Assert.IsNotNull(versioned);
                }
            }

            contacts.Take(20).WriteJsonToFilePath(@"c:\junk\ContactViewModels.json", new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            });
        }

        [TestMethod]
        public void GetRecentContactsTest()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };

            UnixMsTimestamp timeOffsetDate = new UnixMsTimestamp(DateTime.Now.AddHours(-1));
            var ro = service.GetRecentContacts(10, timeOffsetDate, propertyMode: PropertyModeType.value_and_history);
            //var ro = service.GetRecentContacts(20, 1445953483005, propertyMode: PropertyModeType.value_and_history);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactModelList>(data);
                var contacts = dto.contacts.Select(c => (ContactViewModel) c).ToList();
                DateTime? maxTimestamp = new UnixMsTimestamp(contacts.Max(x => x.Properties.First(y => y.Key == "lastmodifieddate").Value));
                DateTime? minTimestamp = new UnixMsTimestamp(contacts.Min(x => x.Properties.First(y => y.Key == "lastmodifieddate").Value));
            }
        }

        [TestMethod]
        public void GetContactByIdFromService()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            //var ro = service.GetContactById(321, props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true);
            var ro = service.GetContactById(4098 ,propertyMode: PropertyModeType.value_and_history, showListMemberships: true);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                File.WriteAllText(@"C:\junk\rawtestcontact.json", data);
                var dto = ClassExtensions.CreateFromJson<ContactModel>(data);
                ContactViewModel vm = dto;
                ContactUpdateModel um = vm;
                um.WriteJsonToFilePath(@"c:\junk\rawtestcontactupdatemodel.json", new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                });
            }
        }

        [TestMethod]
        public void CreateContactTest()
        {
            var service = new ContactService(_hapiKey);
            var contactstring = File.ReadAllText(@"Junk\ContactUpdate.json");
            var ro = service.CreateContact(contactstring);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
            }
        }

        [TestMethod]
        public void UpdateContactTest()
        {
            var service = new ContactService(_hapiKey);
            var contactstring = File.ReadAllText(@"Junk\ContactUpdate2.json");
            var ro = service.UpdateContact(contactstring, 4098);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
            }
        }

        [TestMethod]
        public void DeleteContactTest()
        {
            var service = new ContactService(_hapiKey);
            var ro = service.DeleteContact(4098);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
            }
        }


        //[TestMethod]
        //public void GetAllContactsPagingTest()
        //{
        //    var service = new ContactService(_hapiKey);
        //    //var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };

        //    int? lastId = null;
        //    var moreResults = true;
        //    List<ContactDto> contacts = new List<ContactDto>();

        //    while (moreResults)
        //    {
        //        var ro = service.GetAllContacts(100, lastId);
        //        if (ro.HasExceptions)
        //        {
        //            Assert.Fail();
        //        }
        //        else
        //        {
        //            var data = ro.ResponseData;
        //            var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
        //            moreResults = dto.has_more;
        //            lastId = dto.vid_offset;
        //            contacts.AddRange(dto.contacts);
        //        }
        //    }

        //    contacts.WriteJsonToFilePath(@"c:\junk\allcontacts.json");

        //    //var ro = service.GetAllContacts();
        //    //if (ro.HasExceptions)
        //    //{
        //    //    Assert.Fail();
        //    //}
        //    //else
        //    //{
        //    //    var data = ro.ResponseData;
        //    //    var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
        //    //}
        //}

        //[TestMethod]
        //public void GetAllContactsLeadStatusPagingTest()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "email", "hs_lead_status", "lifecyclestage" };

        //    int? lastId = null;
        //    var moreResults = true;
        //    var contacts = new List<ContactDto>();

        //    while (moreResults)
        //    {
        //        var ro = service.GetAllContacts(100, lastId, properties: props);
        //        if (ro.HasExceptions)
        //        {
        //            Assert.Fail();
        //        }
        //        else
        //        {
        //            var data = ro.ResponseData;
        //            //var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
        //            var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
        //            moreResults = dto.has_more;
        //            lastId = dto.vid_offset;
        //            contacts.AddRange(dto.contacts);
        //        }
        //    }
        //    contacts.WriteJsonToFilePath(@"c:\junk\allpagedcontactsleadstatus.json");
        //}

        //[TestMethod]
        //public void GetAllContactsLeadStatusPagingTest2()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "email", "hs_lead_status", "lifecyclestage" };

        //    int? lastId = null;
        //    var moreResults = true;
        //    var contacts = new List<dynamic>();

        //    while (moreResults)
        //    {
        //        var ro = service.GetAllContacts(100, lastId, properties: props);
        //        if (ro.HasExceptions)
        //        {
        //            Assert.Fail();
        //        }
        //        else
        //        {
        //            var data = ro.ResponseData;
        //            //var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
        //            var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
        //            moreResults = dto.has_more;
        //            lastId = dto.vid_offset;
        //            dynamic o = new ExpandoObject();
        //            contacts.AddRange(dto.contacts.Select(x=> new { lastname = x.properties.lastname, firstname = x.properties.firstname, email = x.properties.email, hs_lead_status = x.properties.hs_lead_status, lifecyclestage = x.properties.lifecyclestage }));
        //        }
        //    }
        //    contacts.WriteJsonToFilePath(@"c:\junk\allpagedcontactsleadstatus2.json");
        //}



        //[TestMethod]
        //public void GetRecentContactsTest()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


        //    var ro = service.GetRecentContacts(20, 1445953483005);
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactListDto>(data);
        //    }
        //}

        //[TestMethod]
        //public void GetContactByIdFromService()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


        //    //var ro = service.GetContactById(321, props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true);
        //    var ro = service.GetContactById(321, showListMemberships: true);
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
        //    }
        //}

        //[TestMethod]
        //public void GetContactsByIdsFromService()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


        //    var ro = service.GetContactsByIds(new[] { 321, 322, 323}, props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true, includeDeleted: true);
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
        //    }
        //}

        //[TestMethod]
        //public void GetContactByEmailFromService()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


        //    var ro = service.GetContactByEmail(@"ssalerno@ami-partners.com", props);
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactDto>(data);

        //        //var optout = data.properties.hs_email_optout_636817;
        //        //DateTime? created = new UnixMsTimestamp(data.properties.createdate);
        //        //var value = optout.value;
        //    }
        //}


        //[TestMethod]
        //public void GetContactByEmailFromService2()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


        //    var ro = service.GetContactByEmail(@"ssalerno@ami-partners.com", props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships:true);
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
        //    }
        //}

        //[TestMethod]
        //public void GetContactsByEmailFromService()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


        //    var ro = service.GetContactsByEmails(new[] {@"ssalerno@ami-partners.com", "dspress@hotmail.com" } , props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true, includeDeleted: true);
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
        //    }
        //}


        //[TestMethod]
        //public void GetContactsByQueryTest()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


        //    var ro = service.GetContactsByQuery(@".net");
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
        //    }
        //}


        //[TestMethod]
        //public void GetContactByTokenIdTest()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };

        //    // TODO: Find a token that works?
        //    var ro = service.GetContactByTokenId("dd28277a8c9a43f6a55da0a7dc9f0e89", props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true);
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
        //    }
        //}

        //[TestMethod]
        //public void GetContactsByTokenIdsTest()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };
        //    var tokenIds = new List<string> { "dd28277a-8c9a-43f6-a55d-a0a7dc9f0e89" }; // TODO: Find a token that works?

        //    var ro = service.GetContactsByTokenIds(tokenIds, props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true, includeDeleted:true);
        //    if (ro.HasExceptions)
        //    {
        //        Assert.Fail();
        //    }
        //    else
        //    {
        //        var data = ro.ResponseData;
        //        var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
        //    }
        //}
    }
}
