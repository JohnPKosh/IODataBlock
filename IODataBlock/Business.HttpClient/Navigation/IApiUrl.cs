using System.Collections.Generic;

namespace Business.HttpClient.Navigation
{
    public interface IApiUrl
    {
        string Root { get; set; }

        IEnumerable<string> PathSegments { get; set; }

        IDictionary<string, object> QueryParams { get; set; }
    }
}