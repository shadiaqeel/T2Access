using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using T2Access.Security.Tokenization.Services;

namespace T2Access.API.Helper
{
    public static class AuthorizationFactory
    {

        public static IAuthService GetAuthrization()
        {
            return new JWTService();
        }

    }
}