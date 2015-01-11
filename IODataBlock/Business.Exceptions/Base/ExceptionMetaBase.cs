using System;
using Business.Exceptions.Interfaces;
using Business.Utilities;

namespace Business.Exceptions.Base
{
    public class ExceptionMetaBase : IExceptionMeta
    {
        public DateTime DateCreatedUtc { get; set; }

        public string HostAssemblyName { get; set; }

        public string HostComputerName { get; set; }

        public string HostUserName { get; set; }

        public string HostUserDomain { get; set; }

        public string MemberName { get; set; }

        public string MessageDetail { get; set; }

        public string ParentName { get; set; }

        public string TypeName { get; set; }

        public static IExceptionMeta CreateDefaultMeta()
        {
            var meta = new ExceptionMetaBase
            {
                DateCreatedUtc = DateTime.UtcNow,
                HostAssemblyName = EnvironmentUtilities.GetAssemblyName(),
                HostComputerName = EnvironmentUtilities.GetComputerName(),
                HostUserName = EnvironmentUtilities.GetUserName(),
                HostUserDomain = EnvironmentUtilities.GetUserDomain()
            };
            return meta;
        }
    }
}