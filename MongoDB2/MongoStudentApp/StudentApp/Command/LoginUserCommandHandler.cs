using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentApp.Command
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Object>
    {
        public Task<object> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public class CommandValidator : AbstractValidator<LoginUserCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
    }
}
