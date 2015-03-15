using Business.Common.Requests;

namespace Business.Common.System
{
    public interface ICommandObjectFactory
    {
        ICommandObject Create(IRequestObject requestObject);
    }
}