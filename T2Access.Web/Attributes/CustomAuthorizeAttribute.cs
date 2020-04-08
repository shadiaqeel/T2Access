using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using T2Access.Security.Tokenization.Services;
using T2Access.Web.Helper;

namespace T2Access.Web.Attributes
{

    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        private readonly IAuthService authService = AuthrizationFactory.GetAuthrization();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {




            var token = httpContext.Session["Token"];

            if (string.IsNullOrEmpty((string)token) || !authService.IsTokenValid((string)token))
            {
                httpContext.Session.Clear();

                return false;
            }



            var username = (string)httpContext.Session["Username"];
            var role = (string)httpContext.Session["Role"];

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(role))
            {
                IPrincipal principal = new GenericPrincipal(new GenericIdentity(username), new string[] { role });
                Thread.CurrentPrincipal = principal;
                httpContext.User = principal;
            }



            return true;
        }



        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            var returnUrl = filterContext.HttpContext.Request.Url;
            //var returnUrl = filterContext.HttpContext.Request.Url;
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Login" }, { "controller", "Account" }, { "returnUrl", returnUrl } });


        }





    }
}