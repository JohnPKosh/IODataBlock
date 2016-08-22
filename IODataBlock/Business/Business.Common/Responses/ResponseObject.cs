using Business.Common.Exceptions;
using Newtonsoft.Json;
using System;

namespace Business.Common.Responses
{
    public class ResponseObject : IResponseObject
    {
        #region Fields and Properties

        public object RequestData { get; set; }

        public string CorrelationId { get; set; }

        public object ResponseData { get; set; }

        public IResponseCode ResponseCode { get; set; }

        public bool HasExceptions => ExceptionCount != 0;

        public int ExceptionCount => ExceptionList?.Exceptions.Count ?? 0;

        public IExceptionObjectList ExceptionList { get; set; }

        #endregion Fields and Properties

        #region Add Exception Methods

        public void AddException(
            Exception exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            if (exception == null)
            {
                return;
            }
            if (ExceptionList == null)
            {
                ExceptionList = new ExceptionObjectListBase(exception, title, description, exceptionGroup, logLevel);
            }
            else
            {
                ExceptionList.Add(exception, title, description, exceptionGroup, logLevel);
            }
        }

        #endregion Add Exception Methods

        #region Utility Methods

        public string ToJson(bool indented = false)
        {
            return indented ? JsonConvert.SerializeObject(this, Formatting.Indented) : JsonConvert.SerializeObject(this, Formatting.None);
        }

        #endregion Utility Methods
    }
}