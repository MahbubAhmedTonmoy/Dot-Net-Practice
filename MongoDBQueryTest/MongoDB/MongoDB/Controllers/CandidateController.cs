using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MongoDB.DTO;
using MongoDB.Model;
using MongoDB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateRepository _repo;
        private readonly ILogger<CandidateController> _logger;

        private readonly IMemoryCache memoryCache; // in-memory caching
        private readonly IDistributedCache distributedCache; // radis caching

        public CandidateController(ILogger<CandidateController> logger, ICandidateRepository repo, IMemoryCache memoryCache, 
            IDistributedCache distributedCache)
        {
            _logger = logger;
            _repo = repo;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }
        [HttpGet("all")]
        public ActionResult GetAllWithOutCache()
        {
            var result = _repo.GetCandidates();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();

        }

        [HttpGet("allinmemorycache")]
        public List<GetCandidateDataDTO> GetAllInMemoryCache()
        {
            //cache
            var cacheKey = "allcandidate";
            if(!memoryCache.TryGetValue(cacheKey, out List<GetCandidateDataDTO> candidatelist))
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
        }

        [HttpGet("allradiscache")]
        public async System.Threading.Tasks.Task<List<GetCandidateDataDTO>> GetAllAsync()
        {
            var cacheKey = "allCandidateUsingRadisCache";
            List<GetCandidateDataDTO> candidates = null;
            string serializedCandidates;
            var encodedCandidates =await distributedCache.GetAsync(cacheKey);
            if(encodedCandidates != null)
            {
                serializedCandidates = Encoding.UTF8.GetString(encodedCandidates);
                candidates = JsonConvert.DeserializeObject<List<GetCandidateDataDTO>>(serializedCandidates);
            }
            else
            {
                candidates = _repo.GetCandidates();
                serializedCandidates = JsonConvert.SerializeObject(candidates);
                encodedCandidates = Encoding.UTF8.GetBytes(serializedCandidates);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddHours(2));
                await distributedCache.SetAsync(cacheKey, encodedCandidates, options);
            }
            return candidates;
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
