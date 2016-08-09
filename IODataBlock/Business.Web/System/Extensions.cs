using System;
using System.Web;

namespace Business.Web.System
{
    public static class Extensions
    {
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

        public static String GetCurrentRootPath()
        {
            return HttpContext.Current.Server.MapPath("~");
        }

        public static String RootPath(this HttpContext context)
        {
            return context.Server.MapPath("~");
        }
    }
}
