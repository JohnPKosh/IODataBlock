using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Extensions;
using HubSpot.Models;
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


            var ro = service.GetAllContacts();
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            }
        }

        [TestMethod]
        public void GetAllContactsPagingTest()
        {
            var service = new ContactService(_hapiKey);
            //var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };

            int? lastId = null;
            var moreResults = true;
            List<ContactDto> contacts = new List<ContactDto>();

            while (moreResults)
            {
                var ro = service.GetAllContacts(100, lastId);
                if (ro.HasExceptions)
                {
                    Assert.Fail();
                }
                else
                {
                    var data = ro.ResponseData;
                    var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
                    moreResults = dto.has_more;
                    lastId = dto.vid_offset;
                    contacts.AddRange(dto.contacts);
                }
            }

            contacts.WriteJsonToFilePath(@"c:\junk\allcontacts.json");

            //var ro = service.GetAllContacts();
            //if (ro.HasExceptions)
            //{
            //    Assert.Fail();
            //}
            //else
            //{
            //    var data = ro.ResponseData;
            //    var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            //}
        }

        [TestMethod]
        public void GetAllContactsLeadStatusPagingTest()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "email", "hs_lead_status", "lifecyclestage" };

            int? lastId = null;
            var moreResults = true;
            var contacts = new List<ContactDto>();

            while (moreResults)
            {
                var ro = service.GetAllContacts(100, lastId, properties: props);
                if (ro.HasExceptions)
                {
                    Assert.Fail();
                }
                else
                {
                    var data = ro.ResponseData;
                    //var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
                    var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                    moreResults = dto.has_more;
                    lastId = dto.vid_offset;
                    contacts.AddRange(dto.contacts);
                }
            }
            contacts.WriteJsonToFilePath(@"c:\junk\allpagedcontactsleadstatus.json");
        }

        [TestMethod]
        public void GetAllContactsLeadStatusPagingTest2()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "email", "hs_lead_status", "lifecyclestage" };

            int? lastId = null;
            var moreResults = true;
            var contacts = new List<dynamic>();

            while (moreResults)
            {
                var ro = service.GetAllContacts(100, lastId, properties: props);
                if (ro.HasExceptions)
                {
                    Assert.Fail();
                }
                else
                {
                    var data = ro.ResponseData;
                    //var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
                    var dto = ClassExtensions.CreateFromJson<ContactListDto>(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                    moreResults = dto.has_more;
                    lastId = dto.vid_offset;
                    dynamic o = new ExpandoObject();
                    contacts.AddRange(dto.contacts.Select(x=> new { lastname = x.properties.lastname, firstname = x.properties.firstname, email = x.properties.email, hs_lead_status = x.properties.hs_lead_status, lifecyclestage = x.properties.lifecyclestage }));
                }
            }
            contacts.WriteJsonToFilePath(@"c:\junk\allpagedcontactsleadstatus2.json");
        }



        [TestMethod]
        public void GetRecentContactsTest()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            var ro = service.GetRecentContacts(20, 1445953483005);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactListDto>(data);
            }
        }

        [TestMethod]
        public void GetContactByIdFromService()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            //var ro = service.GetContactById(321, props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true);
            var ro = service.GetContactById(321, showListMemberships: true);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            }
        }

        [TestMethod]
        public void GetContactsByIdsFromService()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            var ro = service.GetContactsByIds(new[] { 321, 322, 323}, props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true, includeDeleted: true);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            }
        }

        [TestMethod]
        public void GetContactByEmailFromService()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            var ro = service.GetContactByEmail(@"ssalerno@ami-partners.com", props);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);

                //var optout = data.properties.hs_email_optout_636817;
                //DateTime? created = new UnixMsTimestamp(data.properties.createdate);
                //var value = optout.value;
            }
        }


        [TestMethod]
        public void GetContactByEmailFromService2()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            var ro = service.GetContactByEmail(@"ssalerno@ami-partners.com", props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships:true);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            }
        }

        [TestMethod]
        public void GetContactsByEmailFromService()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            var ro = service.GetContactsByEmails(new[] {@"ssalerno@ami-partners.com", "dspress@hotmail.com" } , props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true, includeDeleted: true);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            }
        }


        [TestMethod]
        public void GetContactsByQueryTest()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };


            var ro = service.GetContactsByQuery(@".net");
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            }
        }


        [TestMethod]
        public void GetContactByTokenIdTest()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };

            // TODO: Find a token that works?
            var ro = service.GetContactByTokenId("dd28277a8c9a43f6a55da0a7dc9f0e89", props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            }
        }

        [TestMethod]
        public void GetContactsByTokenIdsTest()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> { "lastname", "firstname", "hs_email_optout_636817" };
            var tokenIds = new List<string> { "dd28277a-8c9a-43f6-a55d-a0a7dc9f0e89" }; // TODO: Find a token that works?

            var ro = service.GetContactsByTokenIds(tokenIds, props, PropertyModeType.value_only, FormSubmissionModeType.All, showListMemberships: true, includeDeleted:true);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);
            }
        }
    }
}
