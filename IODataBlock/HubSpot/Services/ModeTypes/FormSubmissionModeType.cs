namespace HubSpot.Services.ModeTypes
{
    public enum FormSubmissionModeType
    {
        Newest, All, None, Oldest
    }

    public static class FormSubmissionModeTypeExtensions
    {
        public static string AsStringLower(this FormSubmissionModeType value)
        {
            return value.ToString().ToLowerInvariant();
        }
    }
}