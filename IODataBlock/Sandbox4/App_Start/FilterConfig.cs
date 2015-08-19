using System.Web.Mvc;

namespace Sandbox4
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