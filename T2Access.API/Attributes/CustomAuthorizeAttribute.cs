using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using T2Access.API.Helper;
using T2Access.API.Resources;
using T2Access.Security.Tokenization.Services;

namespace T2Access.API.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null || string.IsNullOrEmpty(actionContext.Request.Headers.Authorization.Parameter))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, Resource.NotAuthorized);
                return;
            }


            


            var token = actionContext.Request.Headers.Authorization.Parameter;

            IAuthService authService = AuthrizationFactory.GetAuthrization();

            
            var role = authService.GetTokenClaimValue(token, "Role");

            if (!authService.IsTokenValid(token) || !Roles.Contains(role) )
            {

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                var userName = ((JWTService)authService).GetTokenClaimValue(token, "UserName");


                IPrincipal principal = new GenericPrincipal(new GenericIdentity(userName), new string[] { role });
               
                Thread.CurrentPrincipal = principal;
                actionContext.RequestContext.Principal = principal;

            }


        }





    }
}