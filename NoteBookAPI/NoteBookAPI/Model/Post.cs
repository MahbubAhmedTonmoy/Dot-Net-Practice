using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string status { get; set; }
        public int TotalLike { get; set; }
        public int TotalComment { get; set; }
    }
}
