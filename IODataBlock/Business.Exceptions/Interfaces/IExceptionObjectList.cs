using System;
using System.Collections.Generic;

namespace Business.Exceptions.Interfaces
{
    public interface IExceptionObjectList
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