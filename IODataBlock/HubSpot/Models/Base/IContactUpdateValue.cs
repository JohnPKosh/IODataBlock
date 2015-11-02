using HubSpot.Models.Contacts;

namespace HubSpot.Models.Base
{
    public interface IContactUpdateValue
    {
        string Key { get; set; }
        string Value { get; set; }
        ContactPropertyDto PropertyType { get; set; }
    }
}