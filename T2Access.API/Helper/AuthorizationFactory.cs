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