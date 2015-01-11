using System;
using System.Collections.Generic;
using System.Linq;
using Business.Exceptions.Interfaces;
using Newtonsoft.Json;

namespace Business.Exceptions.Base
{
    public class ExceptionObjectListBase : IExceptionObjectList
    {
        #region Class Inititalization

        public ExceptionObjectListBase(IExceptionMeta meta = null)
        {
            Meta = meta;
            Exceptions = new List<IExceptionObject>();
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

        public static ExceptionObjectListBase Create(IEnumerable<Exception> exceptions, IExceptionMeta meta = null)
        {
            return new ExceptionObjectListBase(exceptions, meta);
        }

        public static ExceptionObjectListBase CreateSystemExceptionList(IEnumerable<Exception> exceptions,
            String memberName = null,
            String messageDetail = null,
            String typeName = null,
            String parentName = null,
            DateTime? dateCreatedUtc = null,
            String hostAssemblyName = null,
            String hostComputerName = null)
        {
            return new ExceptionObjectListBase(exceptions, new ExceptionMetaBase
            {
                MemberName = memberName,
                MessageDetail = messageDetail,
                ParentName = parentName,
                TypeName = typeName,
                DateCreatedUtc = dateCreatedUtc.HasValue ? dateCreatedUtc.Value : DateTime.UtcNow,
                HostAssemblyName = hostAssemblyName,
                HostComputerName = hostComputerName
            });
        }

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