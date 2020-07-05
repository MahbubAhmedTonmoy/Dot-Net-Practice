using IdentityMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Repository
{
    public interface IUserRepository
    {
        User GetUser(string email);
        void InsertUser(User user);
    }
}
