using MongoDB.Bson;
using MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.DTO
{
    public class CandidateSearchPermsDTO
    {
        public List<string> Functions { get; set; }
        public List<string> Locations { get; set; }
        public List<string> CmpSector { get; set; }
    }

    public class CandidateSearchResult
    {
        public string _id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class CandidateSearchResponse
    {
        public List<CandidateSearchResult> Data { get; set; }
        public int TotalCount { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }


}
