using GeaphQL2.DataContext;
using GeaphQL2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeaphQL2.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _db;
        public AuthorRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Author CreateAuthor(Author author)
        {
            author.Id = Guid.NewGuid().ToString();
            _db.Add(author);
            _db.SaveChanges();
            return author;
        }

        public List<Author> GetAllAuthors()
        {
            return _db.Authors.Include(x => x.Books).ToList();
        }
        public Author GetAuthorById(string id)
        {
            return _db.Authors.Include(a => a.Books).Where(a => a.Id == id).FirstOrDefault();
        }
       
    }
}
