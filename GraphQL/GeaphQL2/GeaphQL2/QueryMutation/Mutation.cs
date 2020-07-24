using GeaphQL2.Models;
using GeaphQL2.Models.Types;
using GeaphQL2.Repository;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeaphQL2.QueryMutation
{
    public class Mutation : ObjectGraphType
    {
        public Mutation(IAuthorRepository _repo)
        {
            Field<AuthorType>(
             "AuthorInput",
              arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AuthorInputType>> { Name = "author" }),
              resolve: context =>
              {
                  var author = context.GetArgument<Author>("author");
                  return _repo.CreateAuthor(author);
              });
        }
      
    }
}
