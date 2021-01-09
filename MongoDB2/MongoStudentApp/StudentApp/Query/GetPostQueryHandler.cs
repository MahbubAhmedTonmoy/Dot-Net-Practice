using Entity;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using StudentApp.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentApp.Query
{
    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, object>
    {
        private readonly IRepository repository;

        public GetPostQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<object> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var ProjectDictionary = new Dictionary<string, object>
            {
                { "PostDetails", 1 },
                { "PostedBy.Email", 1},
                { "Comment.CommentDetails", 1 },
                { "Comment.CommentedAt", 1 }
            };
            var filter = GetFilter(request);
            var queryresult = ((Repository)this.repository).GetCollection<Post>()
                .Aggregate(new AggregateOptions { AllowDiskUse = true })
                .Skip(request.PageNumber * request.PageSize)
                .Limit(request.PageSize)
                .Lookup("Users", $"{nameof(Post.UserId)}", "_id", "PostedBy")
                .Match(filter)
                .Lookup("Comments", "_id", "PostId", "Comment")
                .Project(new BsonDocument(ProjectDictionary));
            var a = GetResultAfterQueryExecution(queryresult);
            return a;

        }

        private FilterDefinition<BsonDocument> GetFilter(GetPostQuery request)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            if(request.UserEmail != null)
            {
                var userId = repository.GetItem<User>(x => x.Email == request.UserEmail).ItemId;
                filter &= Builders<BsonDocument>.Filter.Eq("UserId", userId);
            }
            return filter;
        }

        private List<Object> GetResultAfterQueryExecution(IAggregateFluent<BsonDocument> finalQuery)
        {
            var result = new List<Object>();
            try
            {
                if (finalQuery.Any())
                {
                    var resultList = finalQuery.ToList();
                    foreach (var i in resultList)
                    {
                        result.Add(BsonSerializer.Deserialize<object>(i));
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
