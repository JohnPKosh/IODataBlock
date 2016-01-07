using Business.Common.GenericRequests;

namespace Business.Common.Generic
{
    public interface ICommandObjectFactory<TIn, TOut>
    {
        ICommandObject<TIn, TOut> Create(IRequestObject<TIn> requestObject);
    }
}