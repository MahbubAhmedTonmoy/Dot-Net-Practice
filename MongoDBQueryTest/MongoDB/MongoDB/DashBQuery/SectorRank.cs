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

    public class SectorRankParms
    {
        public uint RankByAlphabet { get; set; }
        public uint RankByResult { get; set; } = 1;
    }
    public class SectorRankResult
    {
        public string _id { get; set; }
        public string name { get; set; }
        public int value { get; set; }
    }

    public interface ISectorRank
    {
        List<SectorRankResult> GetSectorRank(SectorRankParms parms);
    }
    public class SectorRank : ISectorRank
    {
        private readonly IMongoCollection<Candidate> collection;
        private readonly IMapper _mapper;
        public SectorRank(IMongoDatabase db, IMapper mapper)
        {
            collection = db.GetCollection<Candidate>(Candidate.DocumentName);
            _mapper = mapper;
        }
        public List<SectorRankResult> GetSectorRank(SectorRankParms parms)
        {
            List<SectorRankResult> functionRankResults = GetActualSectorRank();
            List<SectorRankResult> order = GetOrder(functionRankResults, parms);
            return order;
        }

        private List<SectorRankResult> GetOrder(List<SectorRankResult> functionRankResults, SectorRankParms parms)
        {
            if(parms.RankByResult == 1)
            {
                return functionRankResults.OrderBy(x => x.value).ToList();
            }
            else
            {
                return functionRankResults.OrderBy(x => x.name).ToList();
            }
        }

        private List<SectorRankResult> GetActualSectorRank()
        {
            //db.getCollection('Candidates').aggregate([
            //{$project: { _id: 0, "CandidateId": "$_id", "SelectedCompany":"$SelectedCompanies.OrganizationId"} },
            //{$unwind: "$SelectedCompany"},
            //{ $lookup: { from: "Organizations", localField: "SelectedCompany", foreignField: "OrganizationId", as: "Organization" } },
            //{$unwind: "$Organization"},
            //{$project: { "CandidateId": 1, "Sector":"$Organization.Sector"} },
            //{ $group: { "_id": "$Sector", UniqueCandidates: { $addToSet: "$CandidateId" } } },
            //{$project: { _id: 1,name: "$_id", value: {$size: "$UniqueCandidates"} } },
            //{$match: { "name" : { "$ne": null} } }
            //])
            var ProjectDictionary = new Dictionary<string, object> { 
                { "_id", 0}, 
                { "CandidateId", "$_id" }, 
                { "SelectedCompany", "$SelectedCompanies.OrganizationId" } };
            var ProjectDictionay2 = new Dictionary<string, object> {
                {"CandidateId", 1 },
                { "Sector", "$Organization.Sector"}
            };

            var groupDictionary = new Dictionary<string, object>
            {
                {"_id", "$Sector" },
                { "UniqueCandidates", new BsonDocument( new Dictionary<string, object>(){ { "$addToSet", "$CandidateId" } }) }
            };
            var ProjectDictionay3 = new Dictionary<string, object> {
                {"_id", 1 },
                { "name", "$_id"},
                { "value", new BsonDocument(new Dictionary<string,object>{{ "$size", "$UniqueCandidates" } })}
            };
            var matchDict = new Dictionary<string, object>()
            {
                {"name", new BsonDocument(new Dictionary<string,object>(){{"$ne", null}}) }
            };

            var sectorRankQuery = collection.Aggregate()
                .Project(new BsonDocument(ProjectDictionary))
                .Unwind("SelectedCompany")
                .Lookup("Organizations", "SelectedCompany", "OrganizationId", "Organization")
                .Unwind("Organization")
                .Project(new BsonDocument(ProjectDictionay2))
                .Group(new BsonDocument(groupDictionary))
                .Project(new BsonDocument(ProjectDictionay3))
                .Match(new BsonDocument(matchDict));

            List<SectorRankResult> ranks = new List<SectorRankResult>();
            if (sectorRankQuery.Any())
            {
                var bsonRanks = sectorRankQuery.ToList();
                ranks = BsonSerializer.Deserialize<List<SectorRankResult>>(bsonRanks.ToJson());
            }
            return ranks;
        }
    }
    


}
