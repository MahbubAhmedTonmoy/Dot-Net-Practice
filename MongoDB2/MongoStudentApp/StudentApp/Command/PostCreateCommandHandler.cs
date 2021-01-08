using Entity;
using FluentValidation;
using MediatR;
using StudentApp.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UAM;

namespace StudentApp.Command
{
    public class PostCreateCommandHandler : IRequestHandler<PostCreateCommand, Post>
    {
        private readonly IRepository repository;
        private readonly IJwtGenerator jwtGenerator;

        public PostCreateCommandHandler(IRepository repository, IJwtGenerator jwtGenerator)
        {
            this.repository = repository;
            this.jwtGenerator = jwtGenerator;
        }
        public async Task<Post> Handle(PostCreateCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                PostDetails = request.PostDetails
            };
            this.repository.Save<Post>(post);
            return post;
        }
    }
    public class CommandValidator : AbstractValidator<PostCreateCommand>
    {
        public CommandValidator()
        {
            RuleFor(x => x.PostDetails).NotEmpty();
        }
    }
}
