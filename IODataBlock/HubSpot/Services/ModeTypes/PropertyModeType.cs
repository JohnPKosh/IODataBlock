namespace HubSpot.Services.ModeTypes
{
    public enum PropertyModeType
    {
        value_only,
        value_and_history
    }

    public static class PropertyModeTypeExtensions
    {
        public static string AsStringLower(this PropertyModeType value)
        {
            return value.ToString().ToLowerInvariant();
        }
    }
}