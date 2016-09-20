using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebTrakrData.Model.Dto;
using WebTrakrData.Services;

namespace WebTrackr.Controllers.PublicApi
{
    public class LinkedInProfileController : ApiController
    {

        /// <summary>
        /// Gets the linked in company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="apikey">The apikey.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/LinkedeInProfile/{id:long}/{apikey}")]
        public UserLinkedeInProfileDto Get(long id, string apikey)
        {
            try
            {
                var svc = new UserLinkedInProfileService();
                return svc.GetByLinkedInProfileId(id, apikey);
            }
            catch (Exception ex)
            {

                throw;
            }
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