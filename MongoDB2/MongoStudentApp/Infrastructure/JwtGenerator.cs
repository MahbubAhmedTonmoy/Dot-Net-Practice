using Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace UAM
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JwtGenerator> _logger;
        public JwtGenerator(IConfiguration config, ILogger<JwtGenerator> logger)
        {
            _config = config;
            _logger = logger;
        }
        public TokenResponse CreateToken(User user, string[] role)
        {
            var claims = new[]
            {
                  new Claim(ClaimTypes.NameIdentifier, user.ItemId),
                  new Claim(ClaimTypes.Name, user.FirstName),
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(ClaimTypes.Role, role[0]),
                 // new Claim(ClaimTypes.Role, role[1]),
                        //roleAssigned == Role.User ? new Claim("Create Role", "Create Role") : null,
                        
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //insert information into token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();
            return new TokenResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                expires_in = (tokenDescriptor.Expires).ToString(),
                RefreshToken = refreshToken
            };
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value)),
                    ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals("HS512"))//(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                return principal;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //private void ValidateRefreshToken(User user, string refreshToken)
        //{
        //    if (user == null ||
        //        !user.RefreshToken.Exists(rt => rt.Token == refreshToken))
        //    {
        //        throw new SecurityTokenException("Invalid token!");
        //    }

        //    var storedRefreshToken = user.RefreshTokens.Find(rt => rt.Token == refreshToken);

        //    // Ensure that the refresh token that we got from storage is not yet expired.
        //    if (DateTime.UtcNow > storedRefreshToken.Expiration)
        //    {
        //        throw new SecurityTokenException("Invalid token!");
        //    }
        //}

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
