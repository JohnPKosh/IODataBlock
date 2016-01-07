using System;
using Business.Common.Exceptions;
using Business.Common.Responses;
using Newtonsoft.Json;

namespace Business.Common.GenericResponses
{
    public class ResponseObject<TIn, TOut> : IResponseObject<TIn, TOut>
    {
        #region Class Inititalization

        public ResponseObject()
        {
            //ExceptionList = new ExceptionListBase();
            //Success = false;
        }

        #endregion Class Inititalization

        #region Fields and Properties

        public TIn RequestData { get; set; }

        public string CorrelationId { get; set; }

        public TOut ResponseData { get; set; }

        public IResponseCode ResponseCode { get; set; }

        public bool HasExceptions
        {
            get { return ExceptionCount != 0; }
        }

        public int ExceptionCount
        {
            get { return ExceptionList == null ? 0 : ExceptionList.Exceptions.Count; }
        }

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