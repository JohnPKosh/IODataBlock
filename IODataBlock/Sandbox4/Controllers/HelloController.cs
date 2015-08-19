using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sandbox4.Controllers
{
    /// <summary>
    /// The Hello Controller allows you to test your client API calls and receive a Hello greeting!
    /// </summary>
    public class HelloController : ApiController
    {
        // GET api/<controller>
        /// <summary>
        /// Gets an instance of Hello, World string array! Use this method to test your API code.
        /// </summary>
        /// <returns>string[]</returns>
        public IEnumerable<string> Get()
        {
            return new string[] { "Hello", "World" };
        }

        // GET api/<controller>/5
        /// <summary>
        /// Gets a Hello with the specified identifier. Use this method to test your API code.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>string</returns> /*TODO: Determine if we want this and want to hide generated description */
        public string Get(int id)
        {
            return String.Format(@"Hello #{0}", id);
        }

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}