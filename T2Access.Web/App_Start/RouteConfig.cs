using System.Web.Mvc;
using System.Web.Routing;
using T2Access.Web.Helper;
using T2Access.Web.Attributes;

namespace T2Access.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { lang = "en",controller = "Account", action = "Login", id = UrlParameter.Optional },
                constraints: new { lang = new FromValuesListConstraint("en","ar") }
            ).RouteHandler = new LocalizationHandler() ;
        }
    }
}
