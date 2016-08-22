using Business.Common.GenericResponses;

namespace HubSpot.Services.Forms
{
    public interface IFormsService
    {
        IResponseObject<string, string> GetForms();
    }
}