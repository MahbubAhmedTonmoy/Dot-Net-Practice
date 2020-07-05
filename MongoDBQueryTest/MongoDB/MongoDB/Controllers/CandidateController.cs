using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.DTO;
using MongoDB.Model;
using MongoDB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateRepository _repo;
        private readonly ILogger<CandidateController> _logger;

        public CandidateController(ILogger<CandidateController> logger, ICandidateRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("all")]
        public ActionResult GetAll()
        {
            var result = _repo.GetCandidates();
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] Candidate candidate)
        {
            var alreadyExist = _repo.GetCandidate(candidate.Email);
            if (alreadyExist == null)
            {
                _repo.Create(candidate);
                return Ok(candidate);
            }
            return BadRequest("already exist");
        }

        [HttpPost("createmany")]
        public ActionResult CreateMany([FromBody] List<Candidate> candidates)
        {
            var EmailCheck = new List<string>();
            foreach (var i in candidates)
            {
                EmailCheck.Add(i.Email);
            }
            var alreadyExist = _repo.GetCandidates(EmailCheck);
            if (alreadyExist.Count() == 0)
            {
                _repo.CreateMany(candidates);
                return Ok();
            }
            return BadRequest("already exist");
        }

        [HttpGet("search")]
        public ActionResult<CandidateSearchResponse> Search(CandidateSearchPermsDTO permsDTO)
        {
            var result = _repo.SearchCandidate(permsDTO);
            if(result != null)
            {
                return new CandidateSearchResponse{Data = result,
                    TotalCount = result.Count(),
                    Status = (int)HttpStatusCode.OK,
                    Success = true
                };
            }
            return BadRequest();
        }
    }
}
