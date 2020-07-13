using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GraphQLBlogPost.Models;
using GraphQL.Types;
using GraphQL;
using GraphQLBlogPost.DTO;

namespace GraphQLBlogPost.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _executer;
        public GraphQLController(ISchema schema,
         IDocumentExecuter executer)
        {
            _schema = schema;
            _executer = executer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]GraphQLQueryDTO query)
        {
            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = query.Query;
                _.Inputs = query.Variables?.ToInputs();

            });
            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

    }
}
