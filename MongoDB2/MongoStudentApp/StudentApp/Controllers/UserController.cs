using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentApp.Application;
using StudentApp.Domain.Entity;

namespace StudentApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IRepository repository;

        public UserController(ILogger<UserController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] User user)
        {
            try
            {
                user.ItemId = Guid.NewGuid().ToString();
                this.repository.Save<User>(user);
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok();
        }
    }
}
