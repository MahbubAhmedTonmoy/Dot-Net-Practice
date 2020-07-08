using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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

        private readonly IMemoryCache memoryCache; // in-memory caching

        public CandidateController(ILogger<CandidateController> logger, ICandidateRepository repo, IMemoryCache memoryCache)
        {
            _logger = logger;
            _repo = repo;
            this.memoryCache = memoryCache;
        }

        [HttpGet("all")]
        public List<Candidate> GetAll()
        {
            //cache
            var cacheKey = "allcandidate";
            if(!memoryCache.TryGetValue(cacheKey, out List<Candidate> candidatelist))
            {
                candidatelist = _repo.GetCandidates();
                var cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(30), // Gets or sets an absolute expiration date for the cache entry.
                    Priority = CacheItemPriority.Normal,
                    SlidingExpiration = TimeSpan.FromMinutes(5)  //Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed
                };
                memoryCache.Set(cacheKey, candidatelist, cacheExpirationOptions);
            }
            return candidatelist;

            // --- wih out cache-----
            //var result = _repo.GetCandidates();
            //if(result != null)
            //{
            //    return Ok(result);
            //}
            //return BadRequest();

            //---------------------------
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
