using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.DTO
{
    public class OrganizationSectorSearchDTO
    {
        public string Sector { get; set; }
        public string Name { get; set; }
        public string OrganizationId { get; set; }
    }
    public class SectorSearch
    {
        public List<string> Sector { get; set; }
    }

    public class OrganizationSectorSearchResponse
    {
        public List<OrganizationSectorSearchDTO> Data { get; set; }
        public int TotalCount { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }
}
