using Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    public class UserCreateCommand : IRequest<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
