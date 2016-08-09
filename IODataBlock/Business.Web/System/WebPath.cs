using System.IO;
using System.Linq;

namespace Business.Web.System
{
    public static class WebPath
    {
        public static string Current()
        {
            return Extensions.GetCurrentRootPath();
        }

        public static string Combine(string path1, string path2)
        {
            return Path.Combine(Path.Combine(Extensions.GetCurrentRootPath(), path1), path2);
        }

        public static string Combine(string path1, string path2, string path3)
        {
            return Path.Combine(Path.Combine(Extensions.GetCurrentRootPath(), path1), path2, path3);
        }

        public static string Combine(string path1, string path2, string path3, string path4)
        {
            return Path.Combine(Path.Combine(Extensions.GetCurrentRootPath(), path1), path2, path3, path4);
        }

        public static string Combine(params string[] paths)
        {
            var pathlist = paths.ToList();
            pathlist.Insert(0, Extensions.GetCurrentRootPath());
            return Path.Combine(pathlist.ToArray());
        }
    }
}
