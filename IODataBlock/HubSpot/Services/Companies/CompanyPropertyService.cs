using Business.Common.GenericResponses;
using Flurl;
using Flurl.Http;
using System;

namespace HubSpot.Services.Companies
{
    public class CompanyPropertyService : IPropertyService
    {
        public CompanyPropertyService(string hapikey)
        {
            _hapiKey = hapikey;
        }

        private readonly string _hapiKey;

        public IResponseObject<string, string> GetAllProperties()
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/companies/v2/properties".SetQueryParam("hapikey", _hapiKey);
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