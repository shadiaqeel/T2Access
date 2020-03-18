using T2Access.Security.Tokenization.Services;

namespace T2Access.Web.Helper
{
    internal static class AuthrizationFactory
    {

        internal static IAuthService GetAuthrization()
        {
            return new JWTService();
        }

    }
}