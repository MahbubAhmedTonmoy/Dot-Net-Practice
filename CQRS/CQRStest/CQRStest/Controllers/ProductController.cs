using CQRStest.Commands;
using CQRStest.DTO;
using CQRStest.Model;
using CQRStest.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok();
        }
        [HttpPost("Create")]
        public async Task<ActionResult<Unit>> Create(ProductCreate.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            return await _mediator.Send(new ProductsGetAll.Query());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetbyId(int id)
        {
            return await _mediator.Send(new ProductGetById.Query { Id = id });
        } 

        //https://localhost:5001/api/product/search?s=ban
        [HttpGet("search")]
        public async Task<ActionResult<List<ProductDto>>> Search([FromQuery] string s)
        {
            return await _mediator.Send(new ProductSearch.Query { queryPerameter = s });
        }

        [HttpPost("update/{id}")]
        public async Task<ActionResult<Unit>> Update(int id, ProductUpdate.Command command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            return await _mediator.Send(new ProductDelete.Command { Id = id });
        }
    }
}
