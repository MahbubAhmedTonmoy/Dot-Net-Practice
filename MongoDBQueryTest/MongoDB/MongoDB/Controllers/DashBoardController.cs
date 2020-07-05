using Microsoft.AspNetCore.Mvc;
using MongoDB.DashBQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashBoardController : ControllerBase
    {
        private readonly IFunctionRank _repo;
        private readonly ISectorRank _sectorRank;
        private readonly IOrganizationSelectionRank _organizationSelectionRank;

        public DashBoardController(IFunctionRank repo, ISectorRank sectorRank, IOrganizationSelectionRank organizationSelectionRank)
        {
            _repo = repo;
            _sectorRank = sectorRank;
            _organizationSelectionRank = organizationSelectionRank;
        }
        [HttpGet("function")]
        public ActionResult<DashboardQueryResponse> FunctionRank(FunctionRankParms parms)
        {
            var result = _repo.GetFunctionRank(parms);
            return new DashboardQueryResponse
            {
                Data = result,
                TotalCount = result.Count(),
                Status = (int)HttpStatusCode.OK,
                Success = true
            };
        }

        [HttpGet("sector")]
        public ActionResult<DashboardQueryResponse> SectorRank(SectorRankParms parms)
        {
            var result = _sectorRank.GetSectorRank(parms);
            return new DashboardQueryResponse
            {
                Data = result,
                TotalCount = result.Count(),
                Status = (int)HttpStatusCode.OK,
                Success = true
            };
        }

        [HttpGet("companyselect")]
        public ActionResult<DashboardQueryResponse> CompanySelectRank(OrganizationSelectionParms parms)
        {
            var result = _organizationSelectionRank.GetOrganizationSelectionRank(parms);
            return new DashboardQueryResponse
            {
                Data = result,
                TotalCount = result.Count(),
                Status = (int)HttpStatusCode.OK,
                Success = true
            };
        }

    }
}
