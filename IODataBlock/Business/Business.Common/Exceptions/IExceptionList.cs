using System;
using System.Collections.Generic;

namespace Business.Common.Exceptions
{
    public interface IExceptionList
    {
        IExceptionMeta Meta { get; set; }

        IList<IExceptionObject> Exceptions { get; set; }

        String ToJson(Boolean indented = false);

        void Add(Exception exception);

        void Add(IExceptionObject exception);

        void AddRange(IEnumerable<Exception> exception);

        void AddRange(IEnumerable<IExceptionObject> exception);
    }
}