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
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResponse>
    {
        private readonly IRepository repository;
        private readonly IJwtGenerator jwtGenerator;

        public LoginUserCommandHandler(IRepository repository, IJwtGenerator jwtGenerator)
        {
            this.repository = repository;
            this.jwtGenerator = jwtGenerator;
        }
        public async Task<TokenResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var token = new TokenResponse();
            var user = this.repository.GetItem<User>(x => x.Email == request.Email);
            if(user!= null)
            {
                token = this.jwtGenerator.CreateToken(user, user.Roles.ToArray());
            }
            return token;
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
