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
    public class FunctionRankParms
    {
        public uint RankByAlphabet { get; set; }
        public uint RankByResult { get; set; } = 1;
    }
    public class FunctionRankResult
    {
        public string name { get; set; }
        public int value { get; set; }
    }
    public class DashboardQueryResponse
    {
        public object Data { get; set; }
        public int TotalCount { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }

    public interface IFunctionRank
    {
        List<FunctionRankResult> GetFunctionRank(FunctionRankParms parms);
    }

    public class FunctionRank : IFunctionRank
    {
        private readonly IMongoCollection<Candidate> collection;
        private readonly IMapper _mapper;
        public FunctionRank(IMongoDatabase db, IMapper mapper)
        {
            collection = db.GetCollection<Candidate>(Candidate.DocumentName);
            _mapper = mapper;
        }
        public List<FunctionRankResult> GetFunctionRank(FunctionRankParms parms)
        {
            List<FunctionRankResult> functionRankResults = GetActualFunctionRank();
            var orderedFunctionRanks = SetOrder(functionRankResults, parms);
            return orderedFunctionRanks;
        }

        private List<FunctionRankResult> SetOrder(List<FunctionRankResult> functionRankResults, FunctionRankParms query)
        {
            if (query.RankByAlphabet == 1)
            {
                return functionRankResults.OrderBy(x => x.name).ToList();

            }
            else
            {
                return functionRankResults.OrderByDescending(x => x.value).ToList();
            }
        }

        private List<FunctionRankResult> GetActualFunctionRank()
        {
            //db.getCollection('Candidates').aggregate([
            //{$project: { Functions: 1, _id: 0} },
            //{$unwind: '$Functions'},
            //{$group: { _id: '$Functions', count: { "$sum": 1} } },
            //{$project: { 'Name': '$_id', 'Value': "$count", "_id": 0} }
            //])
            var functionProjectDictionary = new Dictionary<string, object>
            {
               { "Functions", 1 },
               { "_id", 0}
            };
            var groupDictionary = new Dictionary<string, object> {
                {"_id","$Functions" },
                { "count", new BsonDocument("$sum", 1) }
            };
            var resultProjectDic = new Dictionary<string, object>
            {
                { $"{nameof(FunctionRankResult.name)}", "$_id" },
                { $"{nameof(FunctionRankResult.value)}", "$count"},
                { "_id", 0 }
            };

            var finalRank = collection.Aggregate().Project(new BsonDocument(functionProjectDictionary))
                .Unwind("Functions").Group(new BsonDocument(groupDictionary)).Project(new BsonDocument(resultProjectDic));
            return GetResultAfterQueryExecution(finalRank);
        }


        private List<FunctionRankResult> GetResultAfterQueryExecution(IAggregateFluent<BsonDocument> finalRank)
        {
            List<FunctionRankResult> result = new List<FunctionRankResult>();
            if (finalRank.Any())
            {
                var resultList = finalRank.ToList();
                if (resultList != null)
                {
                    result = BsonSerializer.Deserialize<List<FunctionRankResult>>(resultList.ToJson());
                }
            }
            return result;
        }
    }
}
