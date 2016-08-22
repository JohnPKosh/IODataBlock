using System;
using System.IO;
using System.Linq;
using System.Web;

namespace Business.Web.System
{
    public static class Extensions
    {
        #region Path Extensions

        #region Basic Extension Methods

        public static bool TryGetCurrentRootPath(out string path)
        {
            try
            {
                path = GetCurrentRootPath();
                return true;
            }
            catch (Exception)
            {
                path = null;
                return false;
            }
        }

        public static string GetCurrentRootPath()
        {
            return HttpContext.Current.Server.MapPath("~");
        }

        public static string RootPath(this HttpContext context)
        {
            return context.Server.MapPath("~");
        }

        #endregion Basic Extension Methods

        #region Combine Path Extension Methods

        public static string Current()
        {
            return GetCurrentRootPath();
        }

        public static string Combine(string path1, string path2)
        {
            return Path.Combine(Path.Combine(GetCurrentRootPath(), path1), path2);
        }

        public static string Combine(string path1, string path2, string path3)
        {
            return Path.Combine(Path.Combine(GetCurrentRootPath(), path1), path2, path3);
        }

        public static string Combine(string path1, string path2, string path3, string path4)
        {
            return Path.Combine(Path.Combine(GetCurrentRootPath(), path1), path2, path3, path4);
        }

        public static string Combine(params string[] paths)
        {
            var pathlist = paths.ToList();
            pathlist.Insert(0, GetCurrentRootPath());
            return Path.Combine(pathlist.ToArray());
        }

        #endregion Combine Path Extension Methods

        #endregion Path Extensions
    }
}