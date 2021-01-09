using AutoMapper;
using Entity;
using MediatR;
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
    public class CommentController: ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper mapper;

        public CommentController(ILogger<CommentController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment([FromBody] CommentCreateCommand coment)
        {
            try
            {
                return await _mediator.Send(mapper.Map<CommentCreateCommand>(coment));
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
