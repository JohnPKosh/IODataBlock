using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;

namespace Business.HttpClient.Navigation
{
    public interface IApiUrl
    {
        string Root { get; set; }

        IEnumerable<string> PathSegments { get; set; }

        IDictionary<string, object> QueryParams { get; set; }

    }
}
