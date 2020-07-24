using GeaphQL2.DataContext;
using GeaphQL2.Models.Types;
using GeaphQL2.Repository;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeaphQL2.QueryMutation
{
    public class AuthorQuery : ObjectGraphType
    {
        public AuthorQuery(IAuthorRepository _repo)
        {
            Field<AuthorType>(
                "Author",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id", Description = "THe ID of the Author" }),
                resolve: context =>
                {
                    var id = context.GetArgument<string>("id");
                    var author = _repo.GetAuthorById(id);
                    return author;
                });
            Field<ListGraphType<AuthorType>>(
                "Authors",
                resolve: context =>
                {
                    var authors = _repo.GetAllAuthors();
                    return authors;
                });
        }
    }
}
