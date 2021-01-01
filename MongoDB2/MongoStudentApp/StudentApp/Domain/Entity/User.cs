using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.Domain.Entity
{
    [BsonIgnoreExtraElements]
    public class User : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    [BsonIgnoreExtraElements(Inherited = true)]
    public class EntityBase
    {
        [BsonId]
        public string ItemId { get; set; }
    }
}
