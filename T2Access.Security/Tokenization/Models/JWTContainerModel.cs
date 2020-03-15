using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace T2Access.Security.Tokenization.Models
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public Claim[] Claims { get; set; }
        public int ExpireMinutes { get; set; } = 60;
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
    }
}
