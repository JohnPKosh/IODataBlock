using Business.Common.Requests;
using Business.Common.Responses;

namespace Business.Common.System
{
    public interface ICommand
    {
        string CommandName { get; }
        string Description { get; }
        IResponseObject Execute();
    }
}