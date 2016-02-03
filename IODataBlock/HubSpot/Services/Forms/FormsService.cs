using System;
using Business.Common.GenericResponses;
using Flurl;
using Flurl.Http;

namespace HubSpot.Services.Forms
{
    public class FormsService : IFormsService
    {
        public FormsService(string hapikey)
        {
            _hapiKey = hapikey;
        }

        private readonly string _hapiKey;


        public IResponseObject<string, string> GetForms()
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var path = "https://api.hubapi.com/forms/v2/forms".SetQueryParam("hapikey", _hapiKey);
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
