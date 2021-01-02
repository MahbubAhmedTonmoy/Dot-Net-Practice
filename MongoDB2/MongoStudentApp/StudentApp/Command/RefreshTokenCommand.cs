using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UAM;

namespace StudentApp.Command
{
    public class RefreshTokenCommand : IRequest<TokenResponse>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
