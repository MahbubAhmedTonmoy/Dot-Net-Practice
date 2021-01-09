using AutoMapper;
using Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentApp.Command;
using StudentApp.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController: ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper mapper;

        public PostController(ILogger<PostController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateCommand post)
        {
            try
            {
                return await _mediator.Send(mapper.Map<PostCreateCommand>(post));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize(Roles = "user")]
        [HttpPost("getpost")]
        public async Task<ActionResult<Object>> GetPostDetails([FromBody] GetPostQuery query)
        {
            try
            {
                return await _mediator.Send(query);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
