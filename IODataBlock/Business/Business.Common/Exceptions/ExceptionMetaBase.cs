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
        public string HostUserName { get; set; }
        public string HostUserDomain { get; set; }
        public string MemberName { get; set; }
        public string MessageDetail { get; set; }
        public string ParentName { get; set; }
        public string TypeName { get; set; }

        public static IExceptionMeta CreateDefaultMeta()
        {
            var meta = new ExceptionMetaBase();
            meta.DateCreatedUtc = DateTime.UtcNow;
            meta.HostAssemblyName = Utility.EnvironmentUtilities.GetAssemblyName();
            meta.HostComputerName = Utility.EnvironmentUtilities.GetComputerName();
            meta.HostUserName = Utility.EnvironmentUtilities.GetUserName();
            meta.HostUserDomain = Utility.EnvironmentUtilities.GetUserDomain();
            return meta;
        }
    }
}
