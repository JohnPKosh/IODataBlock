using Business.Common.GenericRequests;
using Business.Common.GenericResponses;

namespace Business.Common.Generic
{
    public interface ICommandObjectParser<TIn, TOut>
    {
        ICommandObject<TIn, TOut> Parse(IRequestObject<TIn> requestObject);

        ICommandObject<TIn, TOut> Parse(string commandName, IRequestObject<TIn> requestObject);

        bool TryParse(IRequestObject<TIn> requestObject, out ICommandObject<TIn, TOut> commandObject);

        bool TryParse(string commandName, IRequestObject<TIn> requestObject, out ICommandObject<TIn, TOut> commandObject);

        IResponseObject<TIn, TOut> Execute(IRequestObject<TIn> requestObject);

        IResponseObject<TIn, TOut> Execute(string commandName, TIn requestData, string correlationId = null);
    }
}