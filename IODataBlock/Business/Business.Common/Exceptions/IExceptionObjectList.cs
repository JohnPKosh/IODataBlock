using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Business.Common.Exceptions
{
    public interface IExceptionObjectList
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IList<IExceptionObject> Exceptions { get; set; }

        String ToJson(Boolean indented = false);

        void Add(
            Exception exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            );

        void Add(IExceptionObject exception);

        void AddRange(
            IEnumerable<Exception> exceptions
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            );

        void AddRange(IEnumerable<IExceptionObject> exception);
    }
}