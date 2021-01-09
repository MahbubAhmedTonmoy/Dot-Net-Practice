using GraphQL;
using GraphQL.Types;
using StudentApp.GraphQL.GraphQLQueries;

namespace StudentApp.GraphQL
{
    public class AppSchema : Schema
    {
        public AppSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            //Query = resolver.Resolve<AppQuery>();
            Mutation = resolver.Resolve<AppMutation>();
        }
    }
}
