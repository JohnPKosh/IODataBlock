namespace HubSpot.Models.Properties
{
    public interface IPropertyUpdateValue
    {
        string Key { get; set; }
        string Value { get; set; }
        ContactPropertyTypeModel PropertyType { get; set; }
    }
}