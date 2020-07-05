using AutoMapper;
using MongoDB.DTO;
using MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Organization, OrganizationSectorSearchDTO>();
            CreateMap<Candidate, CandidateSearchResult>();
        }
    }
}
