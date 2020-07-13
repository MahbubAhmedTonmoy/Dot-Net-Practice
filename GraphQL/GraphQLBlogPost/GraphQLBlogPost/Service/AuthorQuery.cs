using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQLBlogPost.Models.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLBlogPost.Service
{
    public class AuthorQuery: ObjectGraphType
    {
        public AuthorQuery(AuthorService authorService)
        {
            int id = 0;
            Field<ListGraphType<AuthorType>>(
            name: "authors", resolve: context =>
            {
                return authorService.GetAllAuthors();
            });
            Field<AuthorType>(
                name: "author",
                arguments: new QueryArguments(new
                QueryArgument<IntGraphType>
                { Name = "id" }),
                resolve: context =>
                {
                    id = context.GetArgument<int>("id");
                    return authorService.GetAuthorById(id);
                }
            );
            Field<ListGraphType<BlogPostType>>(
                name: "blogs",
                arguments: new QueryArguments(new
                QueryArgument<IntGraphType>
                { Name = "id" }),
                resolve: context =>
                {
                    return authorService.GetPostsByAuthor(id);
                }
            );
        }
    }

    public class GraphQLDemoSchema : Schema, ISchema
    {
        public GraphQLDemoSchema(IDependencyResolver
                resolver) : base(resolver)
        {
            Query = resolver.Resolve<AuthorQuery>();
        }
    }
}
