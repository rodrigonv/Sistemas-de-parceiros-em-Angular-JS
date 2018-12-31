using System.Web;
using System.Web.Mvc;

namespace Optimus.Web.Parceiros.RestServer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
