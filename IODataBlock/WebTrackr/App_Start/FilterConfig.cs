using System.Web.Mvc;

namespace WebTrackr
{
    /// <summary>
    /// Web Application FilterConfig
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers the global filters for this Web Application.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}