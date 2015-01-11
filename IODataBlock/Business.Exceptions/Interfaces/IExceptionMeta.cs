using System;

namespace Business.Exceptions.Interfaces
{
    public interface IExceptionMeta
    {
        DateTime DateCreatedUtc { get; set; }

        String HostAssemblyName { get; set; }

        String HostComputerName { get; set; }

        String HostUserName { get; set; }

        String HostUserDomain { get; set; }

        String MemberName { get; set; }

        String MessageDetail { get; set; }

        String ParentName { get; set; }

        String TypeName { get; set; }
    }
}