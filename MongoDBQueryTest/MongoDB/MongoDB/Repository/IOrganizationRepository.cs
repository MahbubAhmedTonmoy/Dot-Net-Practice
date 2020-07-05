using MongoDB.DTO;
using MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public interface IOrganizationRepository
    {
        List<Organization> GetOrganizations();
        List<Organization> GetOrganizations(List<string> id);
        Organization GetOrganizationById(string id);
        void Create(Organization org);
        void CreateMany(List<Organization> org);
        void Update(Organization org);
        void Delete(string organizationId);

        List<OrganizationSectorSearchDTO> GetOrganizationBySector(SectorSearch sectors);
    }
}
