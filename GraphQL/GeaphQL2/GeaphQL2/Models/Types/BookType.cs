using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeaphQL2.Models.Types
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Name = "Book";
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Book.");
            Field(x => x.Name).Description("The name of the Book");
            Field(x => x.Genre).Description("Book genre");
            Field(x => x.Published).Description("If the book is published or not");
        }
    }

    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            Name = "Author";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("Author's ID.");
            Field(x => x.Name).Description("The name of the Author");
            Field(x => x.Books, type: typeof(ListGraphType<BookType>)).Description("Author's books");
        }
    }
    public class AuthorInputType : InputObjectGraphType
    {
        public AuthorInputType()
        {
            Name = "AuthorInput";
            Field<NonNullGraphType<StringGraphType>>("name");
       }
    }
}
