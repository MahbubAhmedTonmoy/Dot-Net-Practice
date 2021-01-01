using Entity;
using FluentValidation;
using MediatR;
using StudentApp.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Command
{
    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, User>
    {
        private readonly IRepository repository;

        public UserCreateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<User> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = new User();
            try
            {
                user.ItemId = Guid.NewGuid().ToString();
                user.Email = request.Email;
                user.Password = request.Password;
                this.repository.Save<User>(user);
            }
            catch (Exception ex)
            {
            }
            return user;
        }

        public class CommandValidator : AbstractValidator<UserCreateCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

    }
}
