using IdentityMicroservice.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> collection;

        public UserRepository(IMongoDatabase db)
        {
            collection = db.GetCollection<User>(User.DocumnetName);
        }
        public User GetUser(string email)
        {
            return collection.Find(x => x.Email == email).FirstOrDefault();
        }

        public void InsertUser(User user)
        {
            collection.InsertOne(user);
        }
    }
}
