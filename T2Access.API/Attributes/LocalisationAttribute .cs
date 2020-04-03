using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace T2Access.API.Attributes
{
    public class LocalisationAttribute : Attribute, IActionFilter
    {
        public const string LangParam = "lang";
        public const string CookieName = "mydomain.CurrentUICulture";

        // List of allowed languages in this app (to speed up check)
        private const string Cultures = "en ar";

        public bool AllowMultiple => true;

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {

            // Try getting culture from URL first
            var culture = (string)actionContext.RequestContext.RouteData.Values[LangParam];

            // If not provided, or the culture does not match the list of known cultures, try cookie or browser setting
            if (string.IsNullOrEmpty(culture) || !Cultures.Contains(culture))
            {

                // load the culture info from the cookie
                var cookie = actionContext.Request.Headers.GetCookies(CookieName).FirstOrDefault();
                if (cookie == null)
                {
                    // set the culture by the location if not specified
                    culture = actionContext.Request.Headers.GetValues("UserLanguages").FirstOrDefault();
                }
                else
                {
                    // set the culture by the cookie content
                    culture = cookie.Cookies[0].Value;

                }
                // set the lang value into route data
                actionContext.RequestContext.RouteData.Values[LangParam] = culture;
            }

            // Keep the part up to the "-" as the primary language
            var language = culture.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0];
            actionContext.RequestContext.RouteData.Values[LangParam] = language;

            // Set the language - ignore specific culture for now
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);



            using (var res = continuation())
            {
                res.Wait();


                // save the locale into cookie (full locale)

                var _cookie = new CookieHeaderValue(CookieName, culture) { Expires = DateTime.Now.AddYears(1), Path = "/", HttpOnly = true };



                res.Result.Headers.AddCookies(new CookieHeaderValue[] { _cookie });

                return res;
            }

        }


    }
}