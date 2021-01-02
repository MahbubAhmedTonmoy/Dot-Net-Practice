using Entity;
using FluentValidation;
using MediatR;
using StudentApp.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            //byte[] passwordHash, passwordSalt;
            //CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
            try
            {
                user.ItemId = Guid.NewGuid().ToString();
                user.Email = request.Email;
                user.Roles = request.Roles;
                user.Password = request.Password;//BitConverter.ToString(passwordHash); 
                this.repository.Save<User>(user);
            }
            catch (Exception ex)
            {
            }
           // var a = VerifyPasswordHash(request.Password, Encoding.ASCII.GetBytes(user.Password), passwordSalt);
            return user;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); 
                for(int i =0; i< computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public class CommandValidator : AbstractValidator<UserCreateCommand>
        {
            private readonly IRepository repository;

            public CommandValidator(IRepository repository)
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("EMAIL_REQUIRED");
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Roles).NotEmpty().Must(IsValidRoles).WithMessage("Not valid role");
                this.repository = repository;
            }

            private bool IsValidRoles(List<string> arg)
            {
                var props = Enum.GetNames(typeof(Role)).ToList();
                foreach (var role in arg)
                {
                    if (!props.Contains(role)) { return false; }
                }
                return true;
            }
        }
        public enum Role
        {
           Admin,
           User
        }
    }
}
