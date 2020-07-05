using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.DTO;
using MongoDB.Model;
using MongoDB.Repository;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository _repo;
        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(ILogger<OrganizationController> logger, IOrganizationRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("{id}")]
        public ActionResult<Organization> Get(string id)
        {
            var result = _repo.GetOrganizationById(id);
            return Ok(result);
        }

        [HttpGet("all")]
        public ActionResult<List<Organization>> GetAll()
        {
            var result = _repo.GetOrganizations();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Organization org)
        {
            var alreadyExist = _repo.GetOrganizationById(org.OrganizationId);
            if (alreadyExist == null)
            {
                _repo.Create(org);
                return CreatedAtAction(nameof(Get), new { id = org.OrganizationId }, org);
            }
            return BadRequest("already exist");
        }
        [HttpPost("createmany")]
        public ActionResult CreateMany([FromBody] List<Organization> org)
        {
            var tempIds = new List<string>();
            foreach(var i in org)
            {
                tempIds.Add(i.OrganizationId);
            }
            var alreadyExist = _repo.GetOrganizations(tempIds);
            if (alreadyExist.Count() == 0)
            {
                _repo.CreateMany(org);
                return Ok();
            }
            return BadRequest("already exist");
        }

        [HttpPut]
        public ActionResult Put([FromBody] Organization org)
        {
            if (org != null)
            {
                _repo.Update(org);
                return new OkResult();
            }
            return new NoContentResult();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _repo.Delete(id);
            return new OkResult();
        }

        [HttpGet("bysector")]
        public ActionResult<OrganizationSectorSearchResponse> GetBySector(SectorSearch s)
        {
            var result = _repo.GetOrganizationBySector(s);
            if(result.Count != 0)
            {
                return new OrganizationSectorSearchResponse
                {
                    Data = result,
                    TotalCount = result.Count(),
                    Status = (int)HttpStatusCode.OK,
                    Success = true
                };
            }
            return BadRequest();
        }
    }
}
