using Business.Common.GenericResponses;

namespace HubSpot.Services
{
    public interface IPropertyService
    {
        IResponseObject<string, string> GetAllProperties();
    }
}
