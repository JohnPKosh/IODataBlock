using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data.DbClient;

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
            return new string[] { "Hello", TestDbClient() };
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

        private string TestDbClient()
        {
            //Data Source=.\EXP14;Initial Catalog=LERG;User ID=servermgr;Password=defr3sTu

            #region sql

            var sql = @"
SELECT [TABLE_CATALOG]
    ,[TABLE_SCHEMA]
    ,[TABLE_NAME]
    ,[COLUMN_NAME]
    ,[ORDINAL_POSITION]
    ,[COLUMN_DEFAULT]
    ,[IS_NULLABLE]
    ,[DATA_TYPE]
    ,[CHARACTER_MAXIMUM_LENGTH]
    ,[CHARACTER_OCTET_LENGTH]
    ,[NUMERIC_PRECISION]
    ,[NUMERIC_PRECISION_RADIX]
    ,[NUMERIC_SCALE]
    ,[DATETIME_PRECISION]
    ,[CHARACTER_SET_CATALOG]
    ,[CHARACTER_SET_SCHEMA]
    ,[CHARACTER_SET_NAME]
    ,[COLLATION_CATALOG]
    ,[COLLATION_SCHEMA]
    ,[COLLATION_NAME]
    ,[DOMAIN_CATALOG]
    ,[DOMAIN_SCHEMA]
    ,[DOMAIN_NAME]
,NULL as testnull
FROM [INFORMATION_SCHEMA].[COLUMNS]
WHERE [TABLE_NAME] LIKE @0
ORDER BY [ORDINAL_POSITION]
";

            #endregion sql

            try
            {
                var data = Database.Query(@"Data Source=.\EXP14;Initial Catalog=LERG;Integrated Security=True;", "System.Data.SqlClient", sql, 120, "LERG%");
                //var data = Database.Query(@"Data Source=CLEHBDB01;Initial Catalog=LERG_Data_2015-09-01;User ID=servermgr;Password=defr3sTu", "System.Data.SqlClient", sql, 120, "LERG%");
                if (data.Any())
                {
                    return data.Count().ToString();
                }
                return "broke";
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}