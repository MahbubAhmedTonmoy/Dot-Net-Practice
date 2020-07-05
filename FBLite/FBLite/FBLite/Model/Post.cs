using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public string UserId { get; set; } // identify which user post this
        public AppUser User { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Like> Likes { get; set; }
    }
}
