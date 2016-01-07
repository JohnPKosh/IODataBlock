using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.GenericResponses;
using Business.Common.Responses;
using Flurl;
using Flurl.Http;

namespace HubSpot.Services
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
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }
    }
}
