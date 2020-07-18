using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using NoteBookAPI.Data;
using NoteBookAPI.Model;
using NoteBookAPI.Observer;
using NoteBookAPI.Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDataContext _db;
        private readonly ILogger<PostRepository> _log;
        public PostRepository(ApplicationDataContext db, ILogger<PostRepository> log) : base(db)
        {
            _db = db;
            _log = log;
        }

        //public void CreatePost(Post post)
        //{
        //    _db.Post.Add(post);
        //}

        //public Post Get(int id)
        //{
        //   return _db.Post.FirstOrDefault(i => i.Id == id);
        //}

        //public async Task<IEnumerable<Post>> GetAll()
        //{
        //    return await _db.Post.ToListAsync();
        //}

        //public async Task Remove(int id)
        //{
        //    var findPost = await  _db.Post.FindAsync(id);
        //    if(findPost != null)
        //    {
        //        _db.Remove(findPost);
        //    }
        //    else
        //    {
        //        _log.LogError($"not found this post {id}");
        //    }
        //}

        public List<Post> Search(string content) 
        {
            var result = _db.Post.Where(x => x.status.Contains(content)).ToList();
            return result;
        }

        public int TotalComment(int PostId)
        {
            var post = Get(PostId);
            int count = Convert.ToInt32(post.TotalComment);
            return count;
        }

        public int TotalLike(int PostId)
        {
            var post = Get(PostId);
            int count = Convert.ToInt32(post.TotalLike);
            return count;
        }

        public void Update(Like like)
        {
            var postAvailable = this.Get(like.PostId);
            postAvailable.TotalLike = postAvailable.TotalLike + 1;
            this.UpdatePost(postAvailable);
        }

        public void UpdatePost(Post post)
        {
            var getpost = _db.Post.FirstOrDefault(i => i.Id == post.Id);
            getpost.status = post.status;
            getpost.TotalLike = post.TotalLike;
            getpost.TotalComment = post.TotalComment;

        }
    }
}
