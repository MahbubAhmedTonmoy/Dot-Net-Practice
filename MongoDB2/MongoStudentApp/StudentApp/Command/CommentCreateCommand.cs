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
    public class CommentCreateCommand : IRequest<Comment>
    {
        public string PostId { get; set; }
        public string CommentDetails { get; set; }
    }
    public class CommentCreateCommandHandler : IRequestHandler<CommentCreateCommand, Comment>
    {
        private readonly IRepository repository;
        private readonly IJwtGenerator jwtGenerator;

        public CommentCreateCommandHandler(IRepository repository, IJwtGenerator jwtGenerator)
        {
            this.repository = repository;
            this.jwtGenerator = jwtGenerator;
        }

        public async Task<Comment> Handle(CommentCreateCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                PostId = request.PostId,
                CommentDetails = request.CommentDetails
            };
            this.repository.Save<Comment>(comment);
            return comment;
        }
    }
    //public class CommandValidator : AbstractValidator<CommentCreateCommand>
    //{
    //    public CommandValidator()
    //    {
    //        RuleFor(x => x.CommentDetails).NotEmpty();
    //    }
    //}
}
