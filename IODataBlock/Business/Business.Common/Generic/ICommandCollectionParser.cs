using Business.Common.GenericRequests;
using Business.Common.GenericResponses;

namespace Business.Common.Generic
{
    public interface ICommandCollectionParser<TIn, TOut>
    {
        ICommandObject<TIn, TOut> Parse(string collectionName, IRequestObject<TIn> requestObject);

        ICommandObject<TIn, TOut> Parse(string collectionName, string commandName, IRequestObject<TIn> requestObject);

        bool TryParse(string collectionName, IRequestObject<TIn> requestObject, out ICommandObject<TIn, TOut> commandObject);

        bool TryParse(string collectionName, string commandName, IRequestObject<TIn> requestObject, out ICommandObject<TIn, TOut> commandObject);

        IResponseObject<TIn, TOut> Execute(string collectionName, IRequestObject<TIn> requestObject);

        IResponseObject<TIn, TOut> Execute(string collectionName, string commandName, TIn requestData, string correlationId = null);
    }
}