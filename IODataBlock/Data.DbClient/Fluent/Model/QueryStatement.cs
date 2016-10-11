using System.Collections.Generic;
using Business.Common.System;
using Newtonsoft.Json;

namespace Data.DbClient.Fluent.Model
{
    public class QueryStatement : ObjectBase<QueryStatement>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SelectColumns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Top { get; set; }

        public string FromTable { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Join> Joins { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Where> WhereFilters { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> GroupBy { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Having> HavingClauses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderBy> OrderByClauses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Skip { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Take { get; set; }

    }
}


/*
SELECT LinkedInFullName
	,LinkedInConnections
	,LinkedInTitle
	,a.LinkedInCompanyName
	, COUNT(*) as cnt
FROM dbo.LinkedInProfile AS a 
LEFT OUTER JOIN dbo.LinkedInCompany as b 
ON a.LinkedInCompanyName = b.LinkedInCompanyName
WHERE LinkedInFullName = 'Jess Gilman' 
AND LinkedInFullName = 'Jess Gilman' 
AND a.[CreatedDate] > '2014-10-10 15:24:18' 
AND a.LinkedInCompanyName = 'T3 Motion'
GROUP BY LinkedInFullName
	, LinkedInConnections
	, LinkedInTitle
	, a.LinkedInCompanyName
HAVING COUNT(*) > 0
ORDER BY LinkedInFullName ASC
OFFSET 2 ROWS 
FETCH NEXT 10 ROWS ONLY 
 */
