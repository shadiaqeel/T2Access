using System.Web;
using System.Web.Mvc;
using T2Access.API.Filters;

namespace T2Access.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //filters.Add(new ModelStateFilter());



        }
    }
}
