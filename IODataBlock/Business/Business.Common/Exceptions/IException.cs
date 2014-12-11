using System;

namespace Business.Common.Exceptions
{
    public interface IException
    {
        IExceptionMeta Meta { get; set; }

        IExceptionObject ExceptionObject { get; set; }

        String ToJson(Boolean indented = false);
    }
}