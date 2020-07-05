using FBLite.DTOs;
using FBLite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.Repository
{
    public interface IPostRepository
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        void Create(Post entity);
        List<Post> Search(string content);
    }
}
