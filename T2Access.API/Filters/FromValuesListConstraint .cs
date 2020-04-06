using System.Linq;
using System.Web;
using System.Web.Routing;

namespace T2Access.API.Filters
{
    public class FromValuesListConstraint : IRouteConstraint
    {
        public FromValuesListConstraint(params string[] values)
        {
            _values = values;
        }

        private readonly string[] _values;

        public bool Match(HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            // Get the value called "parameterName" from the 
            // RouteValueDictionary called "value"
            string value = values[parameterName].ToString();

            // Return true is the list of allowed values contains 
            // this value.
            return _values.Contains(value);
        }
    }
}