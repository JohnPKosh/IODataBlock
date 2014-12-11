using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Common.Exceptions
{
    public class ExceptionMetaBase: IExceptionMeta
    {
        public DateTime DateCreatedUtc { get; set; }
        public string HostAssemblyName { get; set; }
        public string HostComputerName { get; set; }
        public string MemberName { get; set; }
        public string MessageDetail { get; set; }
        public string ParentName { get; set; }
        public string TypeName { get; set; }
    }
}
