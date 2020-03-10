using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ole5.Tokenization;
using Ole5.Tokenization.Services;

namespace T2Access.API.Helper
{
    public static class AuthrizationFactory
    {

        public static IAuthService GetAuthrization()
        {
            return new JWTService();
        }

    }
}