using System;

namespace Business.Common.Exceptions
{
    public class CommandNameNotFoundException : Exception
    {
        public CommandNameNotFoundException(): base() { }

        public CommandNameNotFoundException(string message): base(message) { }

        public CommandNameNotFoundException(string format, params object[] args): base(string.Format(format, args)) { }

        public CommandNameNotFoundException(string message, Exception innerException): base(message, innerException) { }

        public CommandNameNotFoundException(string format, Exception innerException, params object[] args): base(string.Format(format, args), innerException) { }
    }
}