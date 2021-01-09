using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    [BsonIgnoreExtraElements]
    public class Post : EntityBase
    {
        public string PostDetails { get; set; }
        public DateTime PostDate { get; set; } = DateTime.Now;
        public string UserId { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Comment: EntityBase
    {
        public string PostId { get; set; }
        public string CommentDetails { get; set; }
        public DateTime CommentedAt { get; set; } = DateTime.Now;
        public string CommentBy { get; set; }
    }
}
