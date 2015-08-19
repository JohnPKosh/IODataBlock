using System.Collections.Generic;

namespace Sandbox4.Models.Content
{
    public interface IAuthenticatedTargetPath : ITargetPath
    {
        HashSet<AllowedRole> AllowedRoles { get; set; }
        HashSet<AllowedUser> AllowedUsers { get; set; }
    }
}