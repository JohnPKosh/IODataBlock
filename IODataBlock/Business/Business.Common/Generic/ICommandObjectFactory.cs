using Business.Common.Requests;

namespace Business.Common.System
{
    public interface ICommandObjectFactory<TIn, TOut>
    {
        ICommandObject<TIn, TOut> Create(IRequestObject<TIn> requestObject);
    }
}