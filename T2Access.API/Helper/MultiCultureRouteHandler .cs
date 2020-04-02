using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace T2Access.API.Helper
{
    public class MultiCultureRouteHandler : MvcRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var culture = requestContext.RouteData.Values["lang"].ToString();
            var ci = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.TwoLetterISOLanguageName);
            return base.GetHttpHandler(requestContext);
        }
    }
}