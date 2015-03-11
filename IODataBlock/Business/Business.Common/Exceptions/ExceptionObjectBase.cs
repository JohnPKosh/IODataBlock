using System;
using System.Collections.Generic;

namespace Business.Common.Exceptions
{
    public class ExceptionObjectBase : IExceptionObject
    {
        public ExceptionObjectBase(Exception exception, ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error)
        {
            #region System.Exception properties

            LogLevel = logLevel;
            Data = exception.Data as IDictionary<String, Object>;
            HelpLink = exception.HelpLink;
            HResult = exception.HResult;
            InnerExceptionDetail = exception.InnerException == null ? null : new ExceptionObjectBase(exception.InnerException);
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;

            #endregion System.Exception properties
        }

        public ExceptionObjectBase(IExceptionObject exception)
        {
            #region System.Exception properties

            LogLevel = exception.LogLevel;
            Data = exception.Data;
            HelpLink = exception.HelpLink;
            HResult = exception.HResult;
            InnerExceptionDetail = exception.InnerExceptionDetail;
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;

            #endregion System.Exception properties
        }

        public ExceptionObjectBase(
            String message
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            , IDictionary<String, Object> data = null
            , String helpLink = null
            , int hResult = 0
            , IExceptionObject innerExceptionDetail = null
            , String source = null
            , String stackTrace = null
            )
        {
            #region System.Exception properties

            LogLevel = logLevel;
            Data = data;
            HelpLink = helpLink;
            HResult = hResult;
            InnerExceptionDetail = innerExceptionDetail;
            Message = message;
            Source = source;
            StackTrace = stackTrace;

            #endregion System.Exception properties
        }

        #region Factory Method

        public static ExceptionObjectBase Create(Exception exception, ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error)
        {
            return new ExceptionObjectBase(exception);
        }

        public static ExceptionObjectBase Create(IExceptionObject exception, ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error)
        {
            return new ExceptionObjectBase(exception);
        }

        public static ExceptionObjectBase Create(
            String message
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            , IDictionary<String, Object> data = null
            , String helpLink = null
            , int hResult = 0
            , IExceptionObject innerExceptionDetail = null
            , String source = null
            , String stackTrace = null
            )
        {
            return new ExceptionObjectBase(message, logLevel, data, helpLink, hResult, innerExceptionDetail, source, stackTrace);
        }

        #endregion Factory Method

        #region System.Exception properties

        public ExceptionLogLevelType LogLevel { get; set; }

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