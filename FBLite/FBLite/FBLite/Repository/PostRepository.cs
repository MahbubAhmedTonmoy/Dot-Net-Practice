using FBLite.Data;
using FBLite.DTOs;
using FBLite.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FBLite.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _db;

        public PostRepository(AppDbContext db)
        {
            _db = db;
        }
        public void Create(Post entity)
        {
            _db.Posts.Add(entity);
        }

        public IEnumerable<Post> GetAll()
        {
            return _db.Posts.ToList();
        }

        public Post GetById(int id)
        {
            return _db.Posts.FirstOrDefault(i => i.Id == id);
        }

        public List<Post> Search(string content)
        {
            var result = _db.Posts.Where(x => x.Description.Contains(content)).ToList();
            return result;
        }
    }
}
