﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    [BsonIgnoreExtraElements]
    public class User : EntityBase
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public List<string> Roles { get; set; }
        public string RefreshToken { get; set; }
    }

    [BsonIgnoreExtraElements(Inherited = true)]
    public class EntityBase
    {
        [BsonId]
        public string ItemId { get; set; } = Guid.NewGuid().ToString();
    }
}
