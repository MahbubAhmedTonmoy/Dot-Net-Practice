
using Entity;
using System.Security.Claims;

namespace UAM
{
    public interface IJwtGenerator
    {
        TokenResponse CreateToken(User user, string[] role);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken();
    }
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string expires_in { get; set; }
        public string RefreshToken { get; set; }
    }
}
