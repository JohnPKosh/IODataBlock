using System;
using Business.Common.GenericResponses;
using Flurl;
using Flurl.Http;

namespace HubSpot.Services.Contacts
{
    public class ContactPropertyService: IPropertyService
    {
        public ContactPropertyService(string hapikey)
        {
            _hapiKey = hapikey;
        }

        private readonly string _hapiKey;

        public IResponseObject<string, string> GetAllProperties()
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/contacts/v2/properties".SetQueryParam("hapikey", _hapiKey);
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
    }
}
