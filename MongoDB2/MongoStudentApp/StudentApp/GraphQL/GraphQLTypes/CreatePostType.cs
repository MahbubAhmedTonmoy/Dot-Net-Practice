using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.GraphQL.GraphQLTypes
{
    public class CreatePostType : InputObjectGraphType
    {
        public CreatePostType()
        {
            Name = "CreatePostInput";
            Field<NonNullGraphType<StringGraphType>>("PostDetails");
        }
    }
}
