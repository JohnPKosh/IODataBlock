using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Business.Web.System;

namespace BusinessCommon.Controllers
{
    public class AutoDialerResultsController : ApiController
    {
        // GET: api/AutoDialerResults
        public async Task<IHttpActionResult> Get()
        {
            var dir = WebPath.Combine(@"App_Data");
            return Ok(new { DataDirectory = dir });
        }

        // GET: api/AutoDialerResults/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AutoDialerResults
        public void Post(string id, [FromBody]JObject value)
        {
            if (value != null)
            {
                value.Add("id", id);
                var val = value.ToString();
                //var vendorTrunk = value["vendorTrunk"].Value<String>();
                File.WriteAllText(Path.Combine(HttpContext.Current.Server.MapPath("~"), "App_Data", "AutoDialerResults", DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".json"), val);
            }
        }

        // PUT: api/AutoDialerResults/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AutoDialerResults/5
        public void Delete(int id)
        {
        }
    }
}
