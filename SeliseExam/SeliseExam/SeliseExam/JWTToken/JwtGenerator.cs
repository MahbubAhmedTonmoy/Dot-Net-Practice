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
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();
            return new LoginResponseDTO
                { 
                    AccessToken  = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken
                }; 
        }
    }
}
