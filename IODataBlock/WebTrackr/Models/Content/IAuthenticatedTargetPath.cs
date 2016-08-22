using System.Collections.Generic;

namespace WebTrackr.Models.Content
{
    public interface IAuthenticatedTargetPath : ITargetPath
    {
        HashSet<AllowedRole> AllowedRoles { get; set; }
        HashSet<AllowedUser> AllowedUsers { get; set; }
    }
}