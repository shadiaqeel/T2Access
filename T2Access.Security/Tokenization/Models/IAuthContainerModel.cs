using System.Security.Claims;

namespace T2Access.Security.Tokenization.Models
{
    public interface IAuthContainerModel
    {
        Claim[] Claims { get; set; }

        int ExpireMinutes { get; set; }
        string SecurityAlgorithm { get; set; }
    }
}
