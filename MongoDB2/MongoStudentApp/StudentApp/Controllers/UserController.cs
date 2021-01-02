using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command;
using Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentApp.Application;
using StudentApp.Command;
using UAM;

namespace StudentApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(ILogger<UserController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            try
            {
                return await _mediator.Send(new UserCreateCommand { Email = user.Email, Password = user.Password, Roles = user.Roles});
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            try
            {
                return await _mediator.Send(loginUserCommand);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
