using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRStest.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}
