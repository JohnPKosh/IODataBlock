using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Common.Exceptions
{
    public interface IExceptionMeta
    {
        DateTime DateCreatedUtc { get; set; }
        String HostAssemblyName { get; set; }
        String HostComputerName { get; set; }
        String MemberName { get; set; }
        String MessageDetail { get; set; }
        String ParentName { get; set; }
        String TypeName { get; set; }
    }
}
