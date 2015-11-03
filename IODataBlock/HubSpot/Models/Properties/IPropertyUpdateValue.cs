namespace HubSpot.Models.Properties
{
    public interface IPropertyUpdateValue
    {
        string Key { get; set; }
        string Value { get; set; }
        PropertyTypeModel PropertyType { get; set; }
    }
}