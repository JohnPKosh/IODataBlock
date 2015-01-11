using System;
using System.Collections.Generic;

namespace Business.Exceptions.Interfaces
{
    public interface IExceptionObject
    {
        IDictionary<String, Object> Data { get; set; }

        String HelpLink { get; set; }

        int HResult { get; }

        IExceptionObject InnerExceptionDetail { get; }

        String Message { get; set; }

        String Source { get; set; }

        String StackTrace { get; set; }
    }
}