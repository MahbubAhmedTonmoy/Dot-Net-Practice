using Entity;
using FluentValidation;
using MediatR;
using StudentApp.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var checkPassword = VerifyPasswordHash(request.Password);
            if (user!= null && checkPassword)
            {
                token = this.jwtGenerator.CreateToken(user, user.Roles.ToArray());
            }
            return token;
        }
        private bool VerifyPasswordHash(string password)
        {
            byte[] passwordHash, passwordSalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
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
