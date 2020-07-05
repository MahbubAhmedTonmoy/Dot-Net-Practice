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
    public class CandidateRepository : ICandidateRepository
    {
        private readonly IMongoCollection<Candidate> collection;
        private readonly IMapper _mapper;
        public CandidateRepository(IMongoDatabase db, IMapper mapper)
        {
            collection = db.GetCollection<Candidate>(Candidate.DocumentName);
            _mapper = mapper;
        }
        public void Create(Candidate cand)
        {
            collection.InsertOne(cand);
        }

        public void CreateMany(List<Candidate> cand)
        {
            collection.InsertMany(cand);
        }

        public Candidate GetCandidate(string email)
        {
            var result = collection.Find(x => x.Email == email).FirstOrDefault();
            return result;
        }

        public List<Candidate> GetCandidates()
        {
            return collection.Find(FilterDefinition<Candidate>.Empty).ToList();
        }

        public List<Candidate> GetCandidates(List<string> email)
        {
            var result = collection.Find(x => email.Contains(x.Email));
            return result.ToList();
        }


        public void Update(Candidate cand)
        {
            throw new NotImplementedException();
        }


        public List<CandidateSearchResult> SearchCandidate(CandidateSearchPermsDTO parms)
        {
            //db.getCollection('Candidates').aggregate([
            //  {$match: { "Functions": {$in: ["ICT"] } }},
            //   {$match: { "JobLocations": {$in: ["EASTERN_SWITZERLAND"] } }},
            //   {$match: { "SelectedCompanies.Sector": {$in: ["MEDIA"] } }}
            //  ])
            var f = parms.Functions.AsEnumerable(); ;
            var filter = Builders<Candidate>.Filter.In("Functions", parms.Functions);
            var filter2 = Builders<Candidate>.Filter.In("JobLocations", parms.Locations);
            var filter3 = Builders<Candidate>.Filter.In("SelectedCompanies.Sector", parms.CmpSector);

            var ans = collection.Aggregate().Match(filter2).Match(filter).Match(filter3).ToList();
            var result = _mapper.Map<List<Candidate>, List<CandidateSearchResult>>(ans);
            return result;


            //....................wrong way....................
//            db.getCollection('Candidates').aggregate([
//          {$unwind: "$Functions"},
//           {$match: { "Functions": {$in: ["ICT"] }}},
            //{$unwind : "$JobLocations"},
//          {$match: { "JobLocations": {$in: ["EASTERN_SWITZERLAND"] } }},
//          {$unwind : "$SelectedCompanies"},
//          {$match: { "SelectedCompanies.Sector": {$in: ["INFORMATION", "MEDIA"] } }},
//           { $group : {  _id: "$Email",
//                   Email: { $first: "$Email"},
//                   Name : { $first: "$Name"}, }}])

            //List<CandidateSearchResult> result = new List<CandidateSearchResult>();
          
            //var builder = Builders<BsonDocument>.Filter;
            //var filt = builder.In("Functions", parms.Functions);
            //var filt2 = builder.In("JobLocations", parms.Locations);
            //var filt3 = builder.In("SelectedCompanies.Sector", parms.CmpSector);
            //var a = collection.Aggregate().Unwind("Functions")
            //                              .Match(filt)
            //                              .Unwind("JobLocations")
            //                              .Match(filt2)
            //                              .Unwind("SelectedCompanies")
            //                              .Match(filt3)
            //                              .Project("{_id:0}")
            //                              .Group(new BsonDocument {
            //                        { "_id", "$Email" }, { "Email", new BsonDocument("$first", "$Email") },  { "Name", new BsonDocument("$first", "$Name")}
            //                }).ToList();
            
                

            //result = BsonSerializer.Deserialize<List<CandidateSearchResult>>(a.ToJson());
           
            //return result;
        }

      
    }
}
