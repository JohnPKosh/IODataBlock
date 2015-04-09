using Business.Common.Responses;

namespace Business.Common.System
{
    public interface ICommandObject
    {
        string CommandName { get; }

        string Description { get; }

        IResponseObject Execute();
    }
}