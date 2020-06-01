using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace InterviewCommEng.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId {get;set;}

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
