using System.Collections.Generic;

namespace Sandbox3.Models.Content
{
    public class AuthenticatedTargetPath : IAuthenticatedTargetPath
    {
        public string AreaName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Section { get; set; }
        public string ContentId { get; set; }
        public HashSet<AllowedRole> AllowedRoles { get; set; }
        public HashSet<AllowedUser> AllowedUsers { get; set; }
    }
}
