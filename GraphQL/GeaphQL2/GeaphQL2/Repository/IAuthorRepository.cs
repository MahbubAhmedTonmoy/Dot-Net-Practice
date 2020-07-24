using GeaphQL2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeaphQL2.Repository
{
    public interface IAuthorRepository
    {

        List<Author> GetAllAuthors();
        Author GetAuthorById(string id);
    }
}
