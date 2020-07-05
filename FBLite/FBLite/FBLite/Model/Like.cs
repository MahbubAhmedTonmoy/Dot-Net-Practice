using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.Model
{
    public class Like
    {
        public int id { get; set; }
        public string LikerId { get; set; }
        public AppUser Liker { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }

    }
}
