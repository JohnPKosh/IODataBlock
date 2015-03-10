using System;
using System.Collections.Generic;
using Business.Exceptions.Base;
using Newtonsoft.Json;

namespace Business.Common.Responses
{
    public class ResponseMessage
    {
        #region Class Inititalization

        public ResponseMessage()
        {
            //ExceptionList = new ExceptionListBase();
            //Success = false;
        }

        #endregion Class Inititalization

        #region Fields and Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object RequestData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object ResponseData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object ResponseCode { get; set; }

        public bool Success
        {
            get { return ExceptionCount == 0; }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ExceptionCount
        {
            get { return ExceptionList == null ? 0 : ExceptionList.Exceptions.Count; }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ExceptionObjectListBase ExceptionList { get; set; }

        #endregion Fields and Properties

        #region Add Exception Methods

        public void AddException(Exception exception, bool includeDefaultMetaData = false)
        {
            if (exception == null)
            {
                return;
            }
            if (ExceptionList == null)
            {
                ExceptionList = new ExceptionObjectListBase(includeDefaultMetaData ? ExceptionMetaBase.CreateExceptionMeta():null);
            }
            //add error
            ExceptionList.Add(exception);
        }

        //public void AddExceptions(IEnumerable<Exception> exceptions)
        //{
        //    if (exceptions == null)
        //    {
        //        return;
        //    }
        //    if (ExceptionList == null)
        //    {
        //        ExceptionList = new ExceptionObjectListBase();
        //    }
        //    //add error
        //    ExceptionList.AddRange(exceptions);
        //}

        #endregion Add Exception Methods

        #region Utility Methods

        public string ToJson(bool indented = false)
        {
            return indented ? JsonConvert.SerializeObject(this, Formatting.Indented) : JsonConvert.SerializeObject(this, Formatting.None);
        }

        #endregion Utility Methods
    }
}