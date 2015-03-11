using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Business.Common.Exceptions
{
    public interface IExceptionObjectList
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IExceptionMeta Meta { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IList<IExceptionObject> Exceptions { get; set; }

        String ToJson(Boolean indented = false);

        void Add(Exception exception);

        void Add(IExceptionObject exception);

        void AddRange(IEnumerable<Exception> exception);

        void AddRange(IEnumerable<IExceptionObject> exception);
    }
}