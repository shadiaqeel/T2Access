using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace T2Access.Web.Attributes
{
    public class LocalizationHandler : MvcRouteHandler
    {
        public const string LangParam = "lang";

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {

            var language = (string)requestContext.RouteData.Values[LangParam];

            if (language != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            }

            return base.GetHttpHandler(requestContext);

        }
    }
}