using System;

namespace Business.Common.GenericRequests
{
    public static class RequestExtensions
    {
        public static IRequestObject<TNew> TransformRequestData<T, TNew>(this IRequestObject<T> value, Func<T, TNew> transformFunc)
        {
            var rv = new RequestObject<TNew>
            {
                CommandName = value.CommandName,
                CorrelationId = value.CorrelationId,
                DateCreatedUtc = value.DateCreatedUtc,
                Title = value.Title,
                Description = value.Description,
                ExceptionGroup = value.ExceptionGroup,
                HostComputerName = value.HostComputerName,
                HostUserName = value.HostUserName,
                HostUserDomain = value.HostUserDomain,
                ExecutingAssemblyFullName = value.ExecutingAssemblyFullName,
                CallingAssemblyFullName = value.CallingAssemblyFullName,
                EntryAssemblyFullName = value.EntryAssemblyFullName,
                TypeName = value.TypeName,
                MemberName = value.MemberName,
                ParentName = value.ParentName,
                AppId = value.AppId,
                ClientName = value.ClientName,
                ClientIp = value.ClientIp,
            };

            if (value.RequestData == null) return rv;
            rv.RequestData = transformFunc(value.RequestData);
            return rv;
        }
    }
}