using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.DTO;
using MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IMongoCollection<Organization> collection;
        private readonly IMapper _mapper; 
        public OrganizationRepository(IMongoDatabase db, IMapper mapper)
        {
            collection = db.GetCollection<Organization>(Organization.DocumentName);
            _mapper = mapper;
        }
        public void Create(Organization org)
        {
             collection.InsertOne(org); 
            
        }

        public void CreateMany(List<Organization> org)
        {
            collection.InsertMany(org);
        }

        public void Delete(string organizationId)
        {
            collection.DeleteOne(i => i.OrganizationId == organizationId);
        }

        public Organization GetOrganizationById(string id)
        {
            return collection.Find(x => x.OrganizationId == id).FirstOrDefault();
        }

        public List<Organization> GetOrganizations()
        {
            return collection.Find(FilterDefinition<Organization>.Empty).ToList();
            //var result = _col.Find(FilterDefinition<Organization>.Empty).Project(Builders<Organization>.Projection
            //                                        .Exclude("_id")).ToBsonDocument();
            //var myObj = BsonSerializer.Deserialize<Organization>(result);
            //return myObj;
        }

        public List<Organization> GetOrganizations(List<string> id)
        {
            var result = collection.Find(x => id.Contains(x.OrganizationId));
            return result.ToList();
        }

        public void Update(Organization org)
        {
            collection.UpdateOne(i => i.OrganizationId == org.OrganizationId, Builders<Organization>.Update
                .Set(c => c.Name, org.Name)
                .Set(c => c.Address, org.Address)
                .Set(c => c.OrganizationUrl, org.OrganizationUrl)
                .Set(c => c.Sector, org.Sector)
                .Set(c => c.Function, org.Function)
                .Set(c => c.IsActive, org.IsActive)
                );
        }

        public List<OrganizationSectorSearchDTO> GetOrganizationBySector(SectorSearch s)
        {
            //db.getCollection('Organizations').aggregate([
            //    {$match: { "Sector": {$in: ["MEDIA", "AUTOMOBILE"]
            //        } }}])

            //var getSector = collection.Aggregate().Match(x => s.Sector.Contains(x.Sector)).ToList();
            var matchDict = new Dictionary<string, object>()
            {
                {"Sector", new BsonDocument(new Dictionary<string,object>(){{"$in", s.Sector}}) }
            };
            var getSector = collection.Aggregate().Match(new BsonDocument(matchDict)).ToList();
            var result = _mapper.Map<List<Organization>, List<OrganizationSectorSearchDTO>>(getSector);

            //List<OrganizationSectorSearchDTO> result = new List<OrganizationSectorSearchDTO>();
            //foreach(var i in getSector)
            //{
            //    result.Add(new OrganizationSectorSearchDTO
            //    {
            //        Name = i.Name,
            //        OrganizationId = i.OrganizationId,
            //        Sector = i.Sector
            //    });
            //}

            return result;
        }
    }
}
