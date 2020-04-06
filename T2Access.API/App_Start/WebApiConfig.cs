using System.Web.Http;

using T2Access.API.Filters;

namespace T2Access.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                 name: "WebApi",
                 routeTemplate: "api/{lang}/{controller}/{action}/{id}",
                 defaults: new { lang = "en", controller = "Home", action = "Index", id = RouteParameter.Optional },
                constraints: new { lang = new FromValuesListConstraint("en", "ar") },
                handler: new LocalizationHandler(GlobalConfiguration.Configuration)

            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // Validate application model state 
            config.Filters.Add(new ModelStateFilter());
        }
    }
}
