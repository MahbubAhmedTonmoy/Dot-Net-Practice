using Entity;
using GraphQL.Types;
using StudentApp.Application;
using StudentApp.GraphQL.GraphQLTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.GraphQL.GraphQLQueries
{
    public class AppMutation : ObjectGraphType
    {
        public AppMutation(IRepository repository)
        {
            Field<StringGraphType>(
                "CreatePost",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CreatePostType>> { Name = "post" }),
                resolve: context =>
                {
                    var post = context.GetArgument<Post>("post");
                    repository.Save<Post>(post);
                    return $"The post with the id: {post.ItemId} has been successfully deleted from db.";
                });
        }
    }
}
