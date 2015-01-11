using System;
using System.Collections.Generic;
using Business.Exceptions.Interfaces;

namespace Business.Exceptions.Base
{
    public class ExceptionObjectBase : IExceptionObject
    {
        public ExceptionObjectBase(Exception exception)
        {
            #region System.Exception properties

            Data = exception.Data as IDictionary<String, Object>;
            HelpLink = exception.HelpLink;
            HResult = exception.HResult;
            InnerExceptionDetail = exception.InnerException == null ? null : new ExceptionObjectBase(exception.InnerException);
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;

            #endregion System.Exception properties
        }

        #region Factory Method

        public static ExceptionObjectBase Create(Exception exception)
        {
            return new ExceptionObjectBase(exception);
        }

        #endregion Factory Method

        #region System.Exception properties

        public IDictionary<String, Object> Data { get; set; }

        public String HelpLink { get; set; }

        public int HResult { get; private set; }

        public IExceptionObject InnerExceptionDetail { get; private set; }

        public String Message { get; set; }

        public String Source { get; set; }

        public String StackTrace { get; set; }

        #endregion System.Exception properties
    }
}