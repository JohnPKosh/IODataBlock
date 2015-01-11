using System;

namespace Business.Exceptions.Interfaces
{
    public interface IException
    {
        IExceptionMeta Meta { get; set; }

        IExceptionObject ExceptionObject { get; set; }

        String ToJson(Boolean indented = false);
    }
}