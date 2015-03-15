using System;

namespace Business.Common.Requests
{
    public class RequestObject : IRequestObject
    {
        public string CommandName { get; set; }

        public object RequestData { get; set; }

        public string CorrelationId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExceptionGroup { get; set; }
        public string HostComputerName { get; set; }
        public string HostUserName { get; set; }
        public string HostUserDomain { get; set; }
        public string ExecutingAssemblyFullName { get; set; }
        public string CallingAssemblyFullName { get; set; }
        public string EntryAssemblyFullName { get; set; }
        public string TypeName { get; set; }
        public string MemberName { get; set; }
        public string ParentName { get; set; }
        public string AppId { get; set; }
        public string ClientName { get; set; }
        public string ClientIp { get; set; }
    }
}