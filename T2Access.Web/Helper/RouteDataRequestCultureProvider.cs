using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace T2Access.Web.Helper
{
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        public int IndexOfCulture;
        public int IndexofUICulture;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            string culture = null;
            string uiCulture = null;

            var twoLetterCultureName = httpContext.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();
            var twoLetterUICultureName = httpContext.Request.Path.Value.Split('/')[IndexofUICulture]?.ToString();

            if (twoLetterCultureName == "ar")
                culture = "ar";
            else if (twoLetterCultureName == "en")
                culture = uiCulture = "en";

            if (twoLetterUICultureName == "ar")
                culture = "ar";
            else if (twoLetterUICultureName == "en")
                culture = uiCulture = "en";

            if (culture == null && uiCulture == null)
                return NullProviderCultureResult;

            if (culture != null && uiCulture == null)
                uiCulture = culture;

            if (culture == null && uiCulture != null)
                culture = uiCulture;

            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult(providerResultCulture);
        }
    }
}
