using System;
using System.Collections.Generic;

namespace Business.Common.Exceptions
{
    public class ExceptionObjectBase : IExceptionObject
    {
        public ExceptionObjectBase(
            Exception exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            #region System.Exception properties

            Title = title;
            Description = description;
            ExceptionGroup = exceptionGroup;
            LogLevel = logLevel;
            Data = exception.Data as IDictionary<string, object>;
            HelpLink = exception.HelpLink;
            HResult = exception.HResult;
            InnerExceptionDetail = exception.InnerException == null ? null : new ExceptionObjectBase(exception.InnerException);
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;

            #endregion System.Exception properties
        }

        public ExceptionObjectBase(
            string message
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            , IDictionary<string, object> data = null
            , string helpLink = null
            , int hResult = 0
            , IExceptionObject innerExceptionDetail = null
            , string source = null
            , string stackTrace = null
            )
        {
            #region System.Exception properties

            Title = title;
            Description = description;
            ExceptionGroup = exceptionGroup;
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

        public static ExceptionObjectBase Create(
            Exception exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            return new ExceptionObjectBase(exception, title, description, exceptionGroup, logLevel);
        }

        public static ExceptionObjectBase Create(
            string message
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            , IDictionary<string, object> data = null
            , string helpLink = null
            , int hResult = 0
            , IExceptionObject innerExceptionDetail = null
            , string source = null
            , string stackTrace = null
            )
        {
            return new ExceptionObjectBase(message, title, description, exceptionGroup, logLevel, data, helpLink, hResult, innerExceptionDetail, source, stackTrace);
        }

        #endregion Factory Method

        #region System.Exception properties

        public string Title { get; set; }

        public string Description { get; set; }

        public string ExceptionGroup { get; set; }

        public ExceptionLogLevelType LogLevel { get; set; }

        public IDictionary<string, object> Data { get; set; }

        public string HelpLink { get; set; }

        public int HResult { get; private set; }

        public IExceptionObject InnerExceptionDetail { get; private set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        #endregion System.Exception properties
    }
}