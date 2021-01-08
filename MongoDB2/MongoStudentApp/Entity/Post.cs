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
}
