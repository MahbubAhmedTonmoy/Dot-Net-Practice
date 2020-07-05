using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMicroservice.Model;
using IdentityMicroservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middleware;

namespace IdentityMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IJwtBuilder _jwtBuilder;
        private readonly IEncryptor _encryptor;

        public IdentityController(IUserRepository userRepository, IJwtBuilder jwtBuilder, IEncryptor encryptor)
        {
            _userRepository = userRepository;
            _jwtBuilder = jwtBuilder;
            _encryptor = encryptor;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] User user)
        {
            var u = _userRepository.GetUser(user.Email);

            if (u != null)
            {
                return BadRequest("User already exists.");
            }

            user.SetPassword(user.Password, _encryptor);
            _userRepository.InsertUser(user);

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] User user, [FromQuery(Name = "d")] string destination = "frontend")
        {
            var u = _userRepository.GetUser(user.Email);

            if (u == null)
            {
                return NotFound("User not found.");
            }

            if (destination == "backend" && !u.IsAdmin)
            {
                return BadRequest("Could not authenticate user.");
            }

            var isValid = u.ValidatePassword(user.Password, _encryptor);

            if (!isValid)
            {
                return BadRequest("Could not authenticate user.");
            }

            var token = _jwtBuilder.GetToken(u.Id);

            return Ok(token);
        }


    }
}
