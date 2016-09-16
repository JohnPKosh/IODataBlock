using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebTrackr.Controllers.PublicApi
{
    public class FullContactController : ApiController
    {

        // https://api.fullcontact.com/v2/person.json?email=yacj@novozymes.com
        /*
         {
  "status": 404,
  "message": "Searched within last 24 hours. No results found for this Id.",
  "requestId": "68bc8a6a-ec4c-4507-8c25-3699f24c721b"
}
         */

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}