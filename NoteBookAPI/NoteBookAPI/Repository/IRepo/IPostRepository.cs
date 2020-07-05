using NoteBookAPI.Model;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository.IRepo
{
    public interface IPostRepository : IRepository<Post>
    {
        //Post Get(int id);
        //Task<IEnumerable<Post>> GetAll();
        //void CreatePost(Post post);
        void UpdatePost(Post post);
        //Task Remove(int id);
        int TotalLike(int PostId);
        int TotalComment(int PostId);
        List<Post> Search(string content);
    }
}
