using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SeliseExam.DTO;
using SeliseExam.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SeliseExam.JWTToken
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration _config;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ILogger<JwtGenerator> _logger;
        public JwtGenerator(IConfiguration config, ILogger<JwtGenerator> logger, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _config = config;
            _refreshTokenGenerator = refreshTokenGenerator;
            _logger = logger;
        }
        public LoginResponseDTO CreateToken(AppUser user, string[] role)
        {
            var claims = new[]
            {
                  new Claim(ClaimTypes.NameIdentifier, user.Id),
                  new Claim(ClaimTypes.Name, user.UserName),
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
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();
            return new LoginResponseDTO
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
    }
}
