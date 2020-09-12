using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SeliseExam.DTO;
using SeliseExam.JWTToken;
using SeliseExam.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SeliseExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEmailSender _emailSender;
        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager, IConfiguration config, ILogger<AuthController> logger,
             IJwtGenerator jwtGenerator, IEmailSender emailSender)
        {
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
            _logger = logger;
            _emailSender = emailSender;
        }

        

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(UserForRegistrationDTO userRegistrationDto)
        {
            //role create
            if (!await _roleManager.RoleExistsAsync(Role.SuperAdmin))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.SuperAdmin));
            }
            if (!await _roleManager.RoleExistsAsync(Role.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.User));
            }

            //create claim

            //create user
            var userForCreate = new AppUser
            {
                UserName = userRegistrationDto.UserName,
                Name = userRegistrationDto.UserName,
                Email = userRegistrationDto.Email
            };
            if (await _userManager.FindByEmailAsync(userRegistrationDto.Email) != null)
            {
                return BadRequest("this email already used");
            }
            if (userRegistrationDto.Role.Contains("Admin") || userRegistrationDto.Role.Contains("User"))
            {
                var createdUser = await _userManager.CreateAsync(userForCreate, userRegistrationDto.Password);

                // role assign
                if (createdUser.Succeeded)
                {
                    if (userRegistrationDto.Role.Contains("Admin"))
                    {
                        await _userManager.AddToRoleAsync(userForCreate, Role.SuperAdmin);
                    }
                    if (userRegistrationDto.Role.Contains("User"))
                    {
                        await _userManager.AddToRoleAsync(userForCreate, Role.User);
                        //  await _userManager.AddClaimAsync(userForCreate, new Claim("Create Role", "Create Role"));
                    }
                    else
                    {
                        return BadRequest("role is not matched");
                    }

                    // verify email address
                    var user = await _userManager.FindByEmailAsync(userRegistrationDto.Email);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), nameof(AuthController), new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);
                    await _emailSender.SendEmailAsync(message);

                    return Ok(new { createdUser , token});
                }
                else
                {
                    return StatusCode(400, createdUser.Errors);
                }
            }

            return StatusCode(500, "Internal server error");
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Error");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded) return Ok();
            else return BadRequest();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(userForLoginDTO.Email);
                if (userExist == null)
                {
                    return BadRequest($"this {userForLoginDTO.Email} is not resisrered");
                }
                var result = await _signInManager.CheckPasswordSignInAsync(userExist, userForLoginDTO.Password, false);

                if (result.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(userExist);
                    string[] roleAssigned = role.ToArray();

                    return Ok(_jwtGenerator.CreateToken(userExist, roleAssigned));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.StackTrace} {ex.Message}");
                throw;
            }
            return null;// StatusCode(500, "Internal server error");
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDTO refreshTokenDTO)
        {
            try
            {
                var principal = _jwtGenerator.GetPrincipalFromExpiredToken(refreshTokenDTO.Token);
                // email pawa jay na for loop diye
                string email = null;
                var tempClaimPrincipal = principal.Claims.ToList();
                email = tempClaimPrincipal[2].Value;
                //-----
                var userExist = await _userManager.FindByEmailAsync(email);
                if (userExist == null)
                {
                    return BadRequest($"this {email} is not resisrered");
                }
                var role = await _userManager.GetRolesAsync(userExist);
                string[] roleAssigned = role.ToArray();
                return Ok(_jwtGenerator.CreateToken(userExist, roleAssigned));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("not valid input");
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return BadRequest($"{user} not found");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), nameof(AuthController), new { token, email = user.Email }, Request.Scheme);
            var message = new Message(new string[] { "2016100000007@seu.edu.bd" }, "Reset password token", callback, null);
            //return Ok(_emailSender.SendEmailAsync(message));

            return Ok(token);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("not valid input");
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null) return BadRequest($"{user} not found");
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest();
            }
            return Ok();
        }
    }
}
