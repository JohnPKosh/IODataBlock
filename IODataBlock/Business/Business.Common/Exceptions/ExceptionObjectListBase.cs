using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Business.Common.Exceptions
{
    public class ExceptionObjectListBase : IExceptionObjectList
    {
        #region Class Inititalization

        public ExceptionObjectListBase(IExceptionMeta meta = null)
        {
            Meta = meta;
            Exceptions = new List<IExceptionObject>();
        }

        public ExceptionObjectListBase(IExceptionObject exception, IExceptionMeta meta = null)
        {
            Meta = meta;
            Exceptions = new List<IExceptionObject>(new[] { exception });
        }

        public ExceptionObjectListBase(Exception exception, IExceptionMeta meta = null)
        {
            Meta = meta;
            Exceptions = new List<IExceptionObject>(new[] { new ExceptionObjectBase(exception) as IExceptionObject });
        }

        public ExceptionObjectListBase(IEnumerable<IExceptionObject> exceptions, IExceptionMeta meta = null)
        {
            Meta = meta;
            Exceptions = exceptions.ToList();
        }

        public ExceptionObjectListBase(IEnumerable<Exception> exceptions, IExceptionMeta meta = null)
        {
            Meta = meta;
            Exceptions = exceptions.Select(x => new ExceptionObjectBase(x) as IExceptionObject) as IList<IExceptionObject>;
        }

        #endregion Class Inititalization

        #region Fields and Properties

        public IExceptionMeta Meta { get; set; }

        public IList<IExceptionObject> Exceptions { get; set; }

        #endregion Fields and Properties

        #region Factory Method

        #region Standard Factory Methods

        public static ExceptionObjectListBase Create(IExceptionMeta meta = null)
        {
            return new ExceptionObjectListBase(meta);
        }

        public static ExceptionObjectListBase Create(IExceptionObject exception, IExceptionMeta meta = null)
        {
            return new ExceptionObjectListBase(exception, meta);
        }

        public static ExceptionObjectListBase Create(Exception exception, IExceptionMeta meta = null)
        {
            return new ExceptionObjectListBase(exception, meta);
        }

        public static ExceptionObjectListBase Create(IEnumerable<IExceptionObject> exceptions, IExceptionMeta meta = null)
        {
            return new ExceptionObjectListBase(exceptions, meta);
        }

        public static ExceptionObjectListBase Create(IEnumerable<Exception> exceptions, IExceptionMeta meta = null)
        {
            return new ExceptionObjectListBase(exceptions, meta);
        }

        #endregion Standard Factory Methods

        #region Explicit Exception Factory Methods

        public static ExceptionObjectListBase Create(
            String message
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            , IDictionary<String, Object> data = null
            , String helpLink = null
            , int hResult = 0
            , IExceptionObject innerExceptionDetail = null
            , String source = null
            , String stackTrace = null
            , IExceptionMeta meta = null)
        {
            return new ExceptionObjectListBase(ExceptionObjectBase.Create(message, logLevel,data,helpLink,hResult,innerExceptionDetail,source,stackTrace), meta);
        }


        public static ExceptionObjectListBase Create(
            String message
            , ExceptionLogLevelType logLevel = ExceptionLogLevelType.Error
            , IDictionary<String, Object> data = null
            , String helpLink = null
            , int hResult = 0
            , IExceptionObject innerExceptionDetail = null
            , String source = null
            , String stackTrace = null
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , string hostComputerName = null
            , string hostUserName = null
            , string hostUserDomain = null
            , string executingAssemblyFullName = null
            , string callingAssemblyFullName = null
            , string entryAssemblyFullName = null
            , string typeName = null
            , string memberName = null
            , string parentName = null
            , string appId = null
            , string clientName = null
            , string clientIp = null
            , string correlationId = null)
        {
            return new ExceptionObjectListBase(ExceptionObjectBase.Create(message, logLevel, data, helpLink, hResult, innerExceptionDetail, source, stackTrace)
                , ExceptionMetaBase.CreateExceptionMeta(
                    title
                    , description
                    , exceptionGroup
                    , hostComputerName
                    , hostUserName
                    , hostUserDomain
                    , executingAssemblyFullName
                    , callingAssemblyFullName
                    , entryAssemblyFullName
                    , typeName
                    , memberName
                    , parentName
                    , appId
                    , clientName
                    , clientIp
                    , correlationId
                ));
        }


        #endregion

        #region Explicit Meta Factory Methods

        public static ExceptionObjectListBase Create(
            string title = null
            , string description = null
            , string exceptionGroup = null
            , string hostComputerName = null
            , string hostUserName = null
            , string hostUserDomain = null
            , string executingAssemblyFullName = null
            , string callingAssemblyFullName = null
            , string entryAssemblyFullName = null
            , string typeName = null
            , string memberName = null
            , string parentName = null
            , string appId = null
            , string clientName = null
            , string clientIp = null
            , string correlationId = null)
        {
            return new ExceptionObjectListBase(ExceptionMetaBase.CreateExceptionMeta(
                title
                , description
                , exceptionGroup
                , hostComputerName
                , hostUserName
                , hostUserDomain
                , executingAssemblyFullName
                , callingAssemblyFullName
                , entryAssemblyFullName
                , typeName
                , memberName
                , parentName
                , appId
                , clientName
                , clientIp
                , correlationId
                ));
        }

        public static ExceptionObjectListBase Create(
            IExceptionObject exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , string hostComputerName = null
            , string hostUserName = null
            , string hostUserDomain = null
            , string executingAssemblyFullName = null
            , string callingAssemblyFullName = null
            , string entryAssemblyFullName = null
            , string typeName = null
            , string memberName = null
            , string parentName = null
            , string appId = null
            , string clientName = null
            , string clientIp = null
            , string correlationId = null)
        {
            return new ExceptionObjectListBase(exception, ExceptionMetaBase.CreateExceptionMeta(
                title
                , description
                , exceptionGroup
                , hostComputerName
                , hostUserName
                , hostUserDomain
                , executingAssemblyFullName
                , callingAssemblyFullName
                , entryAssemblyFullName
                , typeName
                , memberName
                , parentName
                , appId
                , clientName
                , clientIp
                , correlationId
                ));
        }

        public static ExceptionObjectListBase Create(
            Exception exception
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , string hostComputerName = null
            , string hostUserName = null
            , string hostUserDomain = null
            , string executingAssemblyFullName = null
            , string callingAssemblyFullName = null
            , string entryAssemblyFullName = null
            , string typeName = null
            , string memberName = null
            , string parentName = null
            , string appId = null
            , string clientName = null
            , string clientIp = null
            , string correlationId = null)
        {
            return new ExceptionObjectListBase(exception, ExceptionMetaBase.CreateExceptionMeta(
                title
                , description
                , exceptionGroup
                , hostComputerName
                , hostUserName
                , hostUserDomain
                , executingAssemblyFullName
                , callingAssemblyFullName
                , entryAssemblyFullName
                , typeName
                , memberName
                , parentName
                , appId
                , clientName
                , clientIp
                , correlationId
                ));
        }

        public static ExceptionObjectListBase Create(
            IEnumerable<IExceptionObject> exceptions
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , string hostComputerName = null
            , string hostUserName = null
            , string hostUserDomain = null
            , string executingAssemblyFullName = null
            , string callingAssemblyFullName = null
            , string entryAssemblyFullName = null
            , string typeName = null
            , string memberName = null
            , string parentName = null
            , string appId = null
            , string clientName = null
            , string clientIp = null
            , string correlationId = null)
        {
            return new ExceptionObjectListBase(exceptions, ExceptionMetaBase.CreateExceptionMeta(
                title
                , description
                , exceptionGroup
                , hostComputerName
                , hostUserName
                , hostUserDomain
                , executingAssemblyFullName
                , callingAssemblyFullName
                , entryAssemblyFullName
                , typeName
                , memberName
                , parentName
                , appId
                , clientName
                , clientIp
                , correlationId
                )
                );
        }

        public static ExceptionObjectListBase Create(
            IEnumerable<Exception> exceptions
            , string title = null
            , string description = null
            , string exceptionGroup = null
            , string hostComputerName = null
            , string hostUserName = null
            , string hostUserDomain = null
            , string executingAssemblyFullName = null
            , string callingAssemblyFullName = null
            , string entryAssemblyFullName = null
            , string typeName = null
            , string memberName = null
            , string parentName = null
            , string appId = null
            , string clientName = null
            , string clientIp = null
            , string correlationId = null)
        {
            return new ExceptionObjectListBase(exceptions, ExceptionMetaBase.CreateExceptionMeta(
                title
                , description
                , exceptionGroup
                , hostComputerName
                , hostUserName
                , hostUserDomain
                , executingAssemblyFullName
                , callingAssemblyFullName
                , entryAssemblyFullName
                , typeName
                , memberName
                , parentName
                , appId
                , clientName
                , clientIp
                , correlationId
                )
                );
        }

        #endregion Explicit Meta Factory Methods

        #endregion Factory Method

        #region Add Exception Methods

        public void Add(Exception exception)
        {
            Add(new ExceptionObjectBase(exception));
        }

        public void Add(IExceptionObject exception)
        {
            Exceptions.Add(exception);
        }

        public void AddRange(IEnumerable<Exception> exceptions)
        {
            foreach (var exception in exceptions)
            {
                Add(exception);
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