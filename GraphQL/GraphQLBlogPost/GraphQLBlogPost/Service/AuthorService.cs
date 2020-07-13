using GraphQLBlogPost.Models;
using GraphQLBlogPost.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLBlogPost.Service
{
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;
        public AuthorService(AuthorRepository repository)
        {
            _authorRepository = repository;
        }
        public List<Author> GetAllAuthors()
        {
            return _authorRepository.GetAllAuthors();
        }
        public Author GetAuthorById(int id)
        {
            return _authorRepository.GetAuthorById(id);
        }
        public List<BlogPost> GetPostsByAuthor(int id)
        {
            return _authorRepository.GetPostsByAuthor(id);
        }
    }
}
