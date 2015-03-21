using System.Web;
using System.Web.Mvc;

namespace Zadanie1_db4o
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
