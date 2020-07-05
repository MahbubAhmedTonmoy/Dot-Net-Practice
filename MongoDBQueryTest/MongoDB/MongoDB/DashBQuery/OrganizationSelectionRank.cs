using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.DashBQuery
{
    public class OrganizationSelectionParms
    {
        public uint RankByAlphabet { get; set; }
        public uint RankByResult { get; set; } = 1;
    }
    public class OrganizationSelectionRankResult
    {
        public string _id { get; set; }
        public string name { get; set; }
        public int value { get; set; }
    }
    public interface IOrganizationSelectionRank
    {
        List<OrganizationSelectionRankResult> GetOrganizationSelectionRank(OrganizationSelectionParms parms);
    }
    public class OrganizationSelectionRank : IOrganizationSelectionRank
    {
        private readonly IMongoCollection<Candidate> collection;
        private readonly IMongoCollection<Organization> collection2;
        private readonly IMapper _mapper;
        public OrganizationSelectionRank(IMongoDatabase db, IMapper mapper)
        {
            collection = db.GetCollection<Candidate>(Candidate.DocumentName);
            collection2 = db.GetCollection<Organization>(Organization.DocumentName);
            _mapper = mapper;
        }
        public List<OrganizationSelectionRankResult> GetOrganizationSelectionRank(OrganizationSelectionParms parms)
        {
            List<string> allCompany = GetAllCompany();
            List<OrganizationSelectionRankResult> OrganizationSelectionRankResults = GetActualOrganizationSelectionRank();
            AddMissingCompany(allCompany, OrganizationSelectionRankResults);
            List<OrganizationSelectionRankResult> order = GetOrder(OrganizationSelectionRankResults, parms);
            return order;
        }

        private void AddMissingCompany(List<string> allCompany, List<OrganizationSelectionRankResult> organizationSelectionRankResults)
        {
            if(!allCompany.All(a => organizationSelectionRankResults.Any(r => r._id == a))){
                foreach(string i in allCompany)
                {
                    if(!organizationSelectionRankResults.Any(x => x._id == i))
                    {
                        organizationSelectionRankResults.Add(new OrganizationSelectionRankResult { _id = i, value = 0 });
                    }
                }
            }
        }

        private List<string> GetAllCompany()
        {
            var result = collection2.Distinct(x => x.OrganizationId, FilterDefinition<Organization>.Empty).ToList();
            return result;
        }

        private List<OrganizationSelectionRankResult> GetOrder(List<OrganizationSelectionRankResult> organizationSelectionRankResults, OrganizationSelectionParms parms)
        {
            if (parms.RankByResult == 1)
            {
                return organizationSelectionRankResults.OrderByDescending(x => x.value).ToList();
            }
            else
            {
                return organizationSelectionRankResults.OrderBy(x => x.name).ToList();
            }
        }

        private List<OrganizationSelectionRankResult> GetActualOrganizationSelectionRank()
        {
            //db.getCollection('Candidates').aggregate([
            //{$unwind: "$PreferredCompanies"},
            //{ $lookup: { from: "Organizations", localField: "PreferredCompanies", foreignField: "OrganizationId", as: "Organization" } },
            //{$unwind: "$Organization"},
            //{ $group: { "_id": "$Organization.OrganizationId", name: { $first: "$Organization.Name"}, value: { $sum: 1 } } },
            //])
            var result = collection.Aggregate().Unwind("PreferredCompanies")
                .Lookup("Organizations", "PreferredCompanies", "OrganizationId", "Organization")
                .Unwind("Organization").Group(new BsonDocument
                {
                    { "_id","$Organization.OrganizationId"},
                    { "name", new BsonDocument ("$first", "$Organization.Name") },
                    { "value", new BsonDocument ("$sum", 1) }
                });
            List<OrganizationSelectionRankResult> ranks = new List<OrganizationSelectionRankResult>();
            if (result.Any())
            {
                var bsonRanks = result.ToList();
                ranks = BsonSerializer.Deserialize<List<OrganizationSelectionRankResult>>(bsonRanks.ToJson());
            }
            return ranks;
        }
    }
}
