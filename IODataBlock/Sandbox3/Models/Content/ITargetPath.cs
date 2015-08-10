namespace Sandbox3.Models.Content
{
    public interface ITargetPath
    {
        string AreaName { get; set; }
        string Controller { get; set; }
        string Action { get; set; }
        string Section { get; set; }
        string ContentId { get; set; }
    }
}