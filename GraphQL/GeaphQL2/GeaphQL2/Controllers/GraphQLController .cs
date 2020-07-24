using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeaphQL2.DataContext;
using GeaphQL2.QueryMutation;
using GeaphQL2.Repository;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeaphQL2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphQLController : ControllerBase
    {
        private readonly IAuthorRepository _repo;

        public GraphQLController(IAuthorRepository repo) => _repo = repo;

        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] GraphQLQuery command)
        {
            var inputs = command.Variables.ToInputs();

            var schema = new Schema
            {
                Query = new Query(_repo)
            };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = command.Query;
                _.OperationName = command.OperationName;
                _.Inputs = inputs;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result.Data);
        }

    }
}