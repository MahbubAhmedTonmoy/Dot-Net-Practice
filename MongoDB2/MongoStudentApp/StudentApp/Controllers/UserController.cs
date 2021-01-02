using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper mapper;

        public UserController(ILogger<UserController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            try
            {
                return await _mediator.Send(mapper.Map<UserCreateCommand>(user));
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
