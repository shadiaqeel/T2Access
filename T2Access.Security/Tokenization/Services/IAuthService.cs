using System.Collections.Generic;
using System.Security.Claims;
using T2Access.Security.Tokenization.Models;

namespace T2Access.Security.Tokenization.Services
{
    public interface IAuthService
    {
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
        string GetTokenClaimValue(string token, string claimName);

        bool IsTokenValid(string token);

        ClaimsPrincipal GetPrincipal(string token);



    }
}
