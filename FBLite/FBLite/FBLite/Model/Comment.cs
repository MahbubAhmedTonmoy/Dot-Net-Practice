using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommenterId { get; set; }
        public AppUser Commenter { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
