using Business.Common.Exceptions;
using Business.Common.Responses;
using Newtonsoft.Json;

namespace Business.Common.System
{
    public interface ICommandObject
    {
        string CommandName { get; }

        string Description { get; }

        IResponseObject Execute();
    }
}