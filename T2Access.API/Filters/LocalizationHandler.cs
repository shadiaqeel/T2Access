using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace T2Access.API.Filters
{
    public class LocalizationHandler : DelegatingHandler
    {

        public LocalizationHandler(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration);
        }




        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var language = (string)HttpContext.Current.Request.RequestContext.RouteData.Values["lang"];

            if (language != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            }



            return base.SendAsync(request, cancellationToken);
        }
    }
}