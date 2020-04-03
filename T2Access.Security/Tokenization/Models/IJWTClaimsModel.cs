namespace T2Access.Security.Tokenization.Models
{
    public interface IJWTClaimsModel
    {
        string Username { get; set; }
        string Role { get; set; }
    }
}
