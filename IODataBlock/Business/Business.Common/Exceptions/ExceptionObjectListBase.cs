using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Business.Common.Exceptions
{
    public class ExceptionObjectListBase : IExceptionObjectList
    {
        #region Class Inititalization

        public ExceptionObjectListBase()
        {
            Exceptions = new List<IExceptionObject>();
        }

        public ExceptionObjectListBase(IExceptionObject exception)
        {
            Exceptions = new List<IExceptionObject>(new[] { exception });
        }

        public ExceptionObjectListBase(
            Exception exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            Exceptions = new List<IExceptionObject>(new[] { new ExceptionObjectBase(exception, title, description, exceptionGroup, logLevel) as IExceptionObject });
        }

        public ExceptionObjectListBase(IEnumerable<IExceptionObject> exceptions)
        {
            Exceptions = exceptions.ToList();
        }

        public ExceptionObjectListBase(
            IEnumerable<Exception> exceptions
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            Exceptions = exceptions.Select(x => new ExceptionObjectBase(x, title, description, exceptionGroup, logLevel) as IExceptionObject) as IList<IExceptionObject>;
        }

        #endregion Class Inititalization

        #region Fields and Properties

        public IList<IExceptionObject> Exceptions { get; set; }

        #endregion Fields and Properties

        #region Factory Method

        #region Standard Factory Methods

        public static ExceptionObjectListBase Create()
        {
            return new ExceptionObjectListBase();
        }

        public static ExceptionObjectListBase Create(IExceptionObject exception)
        {
            return new ExceptionObjectListBase(exception);
        }

        public static ExceptionObjectListBase Create(
            Exception exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            return new ExceptionObjectListBase(exception, title, description, exceptionGroup, logLevel);
        }

        public static ExceptionObjectListBase Create(IEnumerable<IExceptionObject> exceptions)
        {
            return new ExceptionObjectListBase(exceptions);
        }

        public static ExceptionObjectListBase Create(
            IEnumerable<Exception> exceptions
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            return new ExceptionObjectListBase(exceptions, title, description, exceptionGroup, logLevel);
        }

        #endregion Standard Factory Methods

        #region Explicit Exception Factory Methods

        public static ExceptionObjectListBase Create(
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
            return new ExceptionObjectListBase(ExceptionObjectBase.Create(message, title, description, exceptionGroup, logLevel, data, helpLink, hResult, innerExceptionDetail, source, stackTrace));
        }

        #endregion

        #endregion Factory Method

        #region Add Exception Methods

        public void Add(
            Exception exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            Add(new ExceptionObjectBase(exception, title, description, exceptionGroup, logLevel));
        }

        public void Add(IExceptionObject exception)
        {
            Exceptions.Add(exception);
        }

        public void AddRange(
            IEnumerable<Exception> exceptions
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            )
        {
            foreach (var exception in exceptions)
            {
                Add(exception, title, description, exceptionGroup, logLevel);
            }
        }

        public void AddRange(IEnumerable<IExceptionObject> exceptions)
        {
            foreach (var exception in exceptions)
            {
                Add(exception);
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