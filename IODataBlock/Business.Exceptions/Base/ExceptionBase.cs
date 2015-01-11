using System;
using Business.Exceptions.Interfaces;
using Newtonsoft.Json;

namespace Business.Exceptions.Base
{
    public class ExceptionBase : IException
    {
        public ExceptionBase(IExceptionMeta meta = null)
        {
            Meta = meta;
        }

        public ExceptionBase(Exception exception, IExceptionMeta meta = null)
        {
            ExceptionObject = ExceptionObjectBase.Create(exception);
            Meta = meta;
        }

        #region Fields and Properties

        public IExceptionMeta Meta { get; set; }

        public IExceptionObject ExceptionObject { get; set; }

        #endregion Fields and Properties

        #region Factory Methods

        public static ExceptionBase Create(Exception exception, IExceptionMeta meta = null)
        {
            return new ExceptionBase(exception, meta);
        }

        public static ExceptionBase CreateSystemException(Exception exception,
            String memberName = null,
            String messageDetail = null,
            String typeName = null,
            String parentName = null,
            DateTime? dateCreatedUtc = null,
            String hostAssemblyName = null,
            String hostComputerName = null
            )
        {
            return new ExceptionBase(exception, new ExceptionMetaBase
            {
                MemberName = memberName,
                MessageDetail = messageDetail,
                ParentName = parentName,
                TypeName = String.IsNullOrWhiteSpace(typeName) ? exception.GetType().Name : typeName,
                DateCreatedUtc = dateCreatedUtc.HasValue ? dateCreatedUtc.Value : DateTime.UtcNow,
                HostAssemblyName = hostAssemblyName,
                HostComputerName = hostComputerName
            });
        }

        #endregion Factory Methods

        public string ToJson(Boolean indented = false)
        {
            return indented ? JsonConvert.SerializeObject(this, Formatting.Indented) : JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}