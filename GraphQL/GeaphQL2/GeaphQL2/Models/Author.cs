using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeaphQL2.Models
{
    public class Author
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
