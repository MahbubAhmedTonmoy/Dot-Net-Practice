using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.DTOs
{
    public class PostViewDTO
    {
        public string Description { get; set; }
        public string PostedBy { get; set; }
        public DateTime PostedDate { get; set; }
        public CommentViewDTO Comment { get; set; }
        public int TotalComment { get; set; }
        public int TotalLike { get; set; }
    }
    public class CommentViewDTO
    {
        public List<string> Description { get; set; }
      //  public string CommenteddBy { get; set; }
    }
}
