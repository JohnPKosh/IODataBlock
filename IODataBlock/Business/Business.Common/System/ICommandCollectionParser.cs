using Business.Common.Requests;
using Business.Common.Responses;

namespace Business.Common.System
{
    public interface ICommandCollectionParser
    {
        ICommandObject Parse(string collectionName, IRequestObject requestObject);

        ICommandObject Parse(string collectionName, string commandName, IRequestObject requestObject);

        IResponseObject Execute(string collectionName, IRequestObject requestObject);

        IResponseObject Execute(string collectionName, string commandName, object requestData, string correlationId = null);
    }
}