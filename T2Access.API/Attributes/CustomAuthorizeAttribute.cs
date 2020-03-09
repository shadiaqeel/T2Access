using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace WebAPI.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {


        public override void OnAuthorization(HttpActionContext actionContext)
        {
             //base.OnAuthorization(actionContext);

            if (actionContext.Request.Headers.Authorization == null)
            {



                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);


            }
            else
            {

                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
               string decodedAuthenicationToken = Encoding.UTF8.GetString (Convert.FromBase64String(authenticationToken));
               string [] usernamePasswordArray = decodedAuthenicationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];



                if (username.Equals("shadi") && password.Equals("123456"))
                {


                  Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {

                    actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);

                }

            }
        }
    }
}