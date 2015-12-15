﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Responses;
using Flurl;
using Flurl.Http;
using HubSpot.Models.Contacts;

namespace HubSpot.Services
{
    public class ContactPropertyService: IContactPropertyService
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
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }
    }
}