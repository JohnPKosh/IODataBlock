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
            Data = exception.Data as IDictionary<String, Object>;
            HelpLink = exception.HelpLink;
            HResult = exception.HResult;
            InnerExceptionDetail = exception.InnerException == null ? null : new ExceptionObjectBase(exception.InnerException);
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;

            #endregion System.Exception properties
        }

        public ExceptionObjectBase(
            String message
            , string title = null
            , string description = null
            , string exceptionGroup = null
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
            String message
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            , IDictionary<String, Object> data = null
            , String helpLink = null
            , int hResult = 0
            , IExceptionObject innerExceptionDetail = null
            , String source = null
            , String stackTrace = null
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