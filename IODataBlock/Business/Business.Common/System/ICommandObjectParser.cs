using Business.Common.Requests;
using Business.Common.Responses;

namespace Business.Common.System
{
    public interface ICommandObjectParser
    {
        ICommandObject Parse(IRequestObject requestObject);

        ICommandObject Parse(string commandName, IRequestObject requestObject);

        IResponseObject Execute(IRequestObject requestObject);

        IResponseObject Execute(string commandName, object requestData, string correlationId = null);
    }
}