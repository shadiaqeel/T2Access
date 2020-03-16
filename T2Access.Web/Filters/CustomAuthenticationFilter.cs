using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using T2Access.Security.Tokenization.Services;
using T2Access.Web.Helper;

namespace T2Access.Web.Filters
{
    public class CustomAuthenticationFilter : AuthorizeAttribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get { return false; } }

        public void OnAuthentication(AuthenticationContext filterContext)
        {

            string authParameter = string.Empty;
            IAuthService authService = AuthrizationFactory.GetAuthrization();



            var authorization = filterContext.RequestContext.HttpContext.Request.Headers.Get("Authorization");

            if (authorization == null)
                return;






        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            throw new NotImplementedException();
        }
    }

    //public class AuthenticationFailureResult 
}