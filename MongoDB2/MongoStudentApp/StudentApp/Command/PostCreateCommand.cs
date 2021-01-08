using Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentApp.Command
{
    public class PostCreateCommand : IRequest<Post>
    {
        public string PostDetails { get; set; }
    }
}
