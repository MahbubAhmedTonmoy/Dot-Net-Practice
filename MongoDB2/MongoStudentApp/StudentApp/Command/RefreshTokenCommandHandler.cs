using Entity;
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
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
    {
        private readonly IRepository repository;
        private readonly IJwtGenerator jwtGenerator;

        public RefreshTokenCommandHandler(IRepository repository, IJwtGenerator jwtGenerator)
        {
            this.repository = repository;
            this.jwtGenerator = jwtGenerator;
        }

        public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = new TokenResponse();
            
            var principal = jwtGenerator.GetPrincipalFromExpiredToken(request.Token);
            // email pawa jay na for loop diye
            string email = null;
            var tempClaimPrincipal = principal.Claims.ToList();
            email = tempClaimPrincipal[2].Value;
            //-----
            var userExist = this.repository.GetItem<User>(x => x.Email == email);
            if (userExist == null || userExist.RefreshToken != request.RefreshToken)
            {
                return null;
            }
            var role = userExist.Roles;
            string[] roleAssigned = role.ToArray();
            token = jwtGenerator.CreateToken(userExist, roleAssigned);
            return token;
        }
    }
}
