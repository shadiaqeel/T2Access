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
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Validate application model state 
            config.Filters.Add(new ModelStateFilter());
        }
    }
}
