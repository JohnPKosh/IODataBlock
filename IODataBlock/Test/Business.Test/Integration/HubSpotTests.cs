using Business.Common.Configuration;
using Business.Common.Extensions;
using Flurl;
using Flurl.Http;
using HubSpot.Models.Contacts;
using HubSpot.Services.Contacts;
using HubSpot.Services.ModeTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Business.Test.Integration
{
    [TestClass]
    public class HubSpotTests
    {
        public HubSpotTests()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
        }

        private readonly string _hapiKey;

        //[TestMethod]
        //public void ReadWriteContact()
        //{
        //    var contact = File.ReadAllText(@"SampleResults\contact.json").ConvertJson<ContactDto>();

        //    Assert.IsNotNull(contact.properties);
        //    Assert.IsNotNull(contact);

        //    contact.WriteJsonToFilePath(@"SampleResults\contactClone.json");
        //}

        //[TestMethod]
        //public void ReadWriteContactById()
        //{
        //    var contact = File.ReadAllText(@"SampleResults\contactById.json").ConvertJson<ContactDto>();

        //    Assert.IsNotNull(contact.properties);
        //    Assert.IsNotNull(contact);

        //    contact.WriteJsonToFilePath(@"SampleResults\contactByIdClone.json");
        //}

        //[TestMethod]
        //public void ReadWriteContactList()
        //{
        //    var contact = File.ReadAllText(@"SampleResults\contactListAll.json").ConvertJson<ContactListDto>();

        //    Assert.IsNotNull(contact);

        //    contact.WriteJsonToFilePath(@"SampleResults\contactListAllClone.json");
        //}

        //[TestMethod]
        //public void ReadWriteContactListQuery()
        //{
        //    var contact = File.ReadAllText(@"SampleResults\contactListQuery.json").ConvertJson<ContactListDto>();

        //    Assert.IsNotNull(contact);

        //    contact.WriteJsonToFilePath(@"SampleResults\contactListQueryClone.json");
        //}

        //[TestMethod]
        //public void GetWithFlurl()
        //{
        //    // https://api.hubapi.com/contacts/v1/contact/vid/3/profile?hapikey=your+key+here

        //    var result = "https://api.hubapi.com/contacts/v1/contact/vid"
        //        .AppendPathSegment("37")
        //        .AppendPathSegment("profile")
        //        .SetQueryParam("hapikey", _hapiKey)
        //        .GetJsonAsync<ContactDto>().Result;

        //    if (result == null) Assert.Fail();
        //    else
        //    {
        //        var vid = result.vid;
        //        Assert.IsNotNull(vid);
        //        result.WriteJsonToFilePath(@"SampleResults\contactByIdClone.json", new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate});
        //    }
        //}

        //[TestMethod]
        //public void GetAllContacts()
        //{
        //    // https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=your+key+here

        //    var result = "https://api.hubapi.com/contacts/v1/lists/all/contacts/all"
        //        .SetQueryParam("hapikey", _hapiKey)
        //        .GetJsonAsync<ContactListDto>().Result;

        //    if (result == null) Assert.Fail();
        //    else
        //    {
        //        //var vid = result.vid;
        //        //Assert.IsNotNull(vid);
        //        result.WriteJsonToFilePath(@"SampleResults\allContacts.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
        //    }
        //}

        [TestMethod]
        public void GetAllContactsDynamic()
        {
            // https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=your+key+here

            var result = "https://api.hubapi.com/contacts/v1/lists/all/contacts/all"
                .SetQueryParam("hapikey", _hapiKey)
                .GetJsonAsync<ExpandoObject>().Result;

            if (result == null) Assert.Fail();
            else
            {
                //var vid = result.vid;
                //Assert.IsNotNull(vid);
                result.WriteJsonToFilePath(@"SampleResults\allContactsdynamic.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }

        //[TestMethod]
        //public void GetContactDynamic()
        //{
        //    // https://api.hubapi.com/contacts/v1/contact/vid/315/profile?hapikey=your+key+here

        //    var result = "https://api.hubapi.com/contacts/v1/contact/vid"
        //        .AppendPathSegment("315")
        //        .AppendPathSegment("profile")
        //        .SetQueryParam("hapikey", _hapiKey)
        //        .GetJsonAsync<ContactDto>().Result;

        //    if (result == null) Assert.Fail();
        //    else
        //    {
        //        var vid = result.vid;
        //        Assert.IsNotNull(vid);
        //        result.WriteJsonToFilePath(@"SampleResults\contactByIdDynamic.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
        //    }
        //}

        [TestMethod]
        public void GetCompanyWithFlurl()
        {
            // https://api.hubapi.com/companies/v2/companies/recent/modified?hapikey=your+key+here

            var result = "https://api.hubapi.com/companies/v2/companies/recent/modified"
                .SetQueryParam("hapikey", _hapiKey)
                .GetJsonAsync<ExpandoObject>().Result;

            if (result == null) Assert.Fail();
            else
            {
                //var vid = result.vid;
                //Assert.IsNotNull(vid);
                result.WriteJsonToFilePath(@"SampleResults\recentCompanies.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }

        [TestMethod]
        public void SaveAllContacts()
        {
            var contacts = GetAllContactViewModels(100, propertyMode: PropertyModeType.value_only);

            contacts.WriteJsonToFile(new FileInfo(@"SampleResults\allContacts.json"));
        }

        [TestMethod]
        public void LoadAllContacts()
        {
            var contacts = new FileInfo(@"SampleResults\allContacts.json").ReadJsonFile<IList<ContactViewModel>>();
            var forms = contacts.Where(x => x.form_submissions.Any()).Select(f => f.form_submissions);
            var formids = new HashSet<Tuple<string, string>>();
            foreach (var form in forms)
            {
                foreach (var f in form)
                {
                    var formId = f.Value<string>("form-id");
                    var title = f.Value<string>("title");
                    formids.Add(new Tuple<string, string>(formId, title));
                }
            }
            formids.WriteJsonToFile(new FileInfo(@"SampleResults\forms.json"));
        }

        // mkanell@ajc.com
        // mfratto@nwc.co

        //[TestMethod]
        //public void GetContactByEmailFromService()
        //{
        //    var service = new ContactService(_hapiKey);
        //    var props = new List<string> {"lastname", "firstname", "hs_email_optout_636817"};

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

        public IEnumerable<ContactViewModel> GetAllContactViewModels(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var service = new ContactService(_hapiKey);
            return service.GetAllContactViewModels(count, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships).ToList();
        }
    }
}