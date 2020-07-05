using FBLite.DTOs;
using FBLite.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FBLite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("ok");
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(UserForRegistrationDTO userRegistrationDto)
        {
            //role create
            if(!await _roleManager.RoleExistsAsync(Role.SuperAdmin))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.SuperAdmin));
            }
            if (!await _roleManager.RoleExistsAsync(Role.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.User));
            }

            //create user
            var userForCreate = new AppUser
            {
                UserName = userRegistrationDto.UserName,
                Email = userRegistrationDto.Email
            };
            if(await _userManager.FindByEmailAsync(userRegistrationDto.Email) != null)
            {
                return BadRequest("this email already used");
            }
            var createdUser = await _userManager.CreateAsync(userForCreate, userRegistrationDto.Password);

            // role assign
            if(createdUser.Succeeded)
            {
                if(userRegistrationDto.Role == "Admin")
                {
                    await _userManager.AddToRoleAsync(userForCreate, Role.SuperAdmin);
                }
                else if (userRegistrationDto.Role == "User")
                {
                    await _userManager.AddToRoleAsync(userForCreate, Role.User);
                }
                else
                {
                    return BadRequest("role is not matched");
                }
                return Ok(createdUser);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userExist = await _userManager.FindByEmailAsync(userForLoginDTO.Email);
            if(userExist == null)
            {
                return BadRequest($"this {userForLoginDTO.Email} is not resisrered");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(userExist, userForLoginDTO.Password, false);

            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(userExist);
                string roleAssigned = role[0];
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userExist.Id),
                    new Claim(ClaimTypes.Name, userExist.UserName),
                    new Claim(ClaimTypes.Email, userExist.Email),
                    new Claim(ClaimTypes.Role, roleAssigned)
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

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }
            return BadRequest("login failed");
        }
    }
}
