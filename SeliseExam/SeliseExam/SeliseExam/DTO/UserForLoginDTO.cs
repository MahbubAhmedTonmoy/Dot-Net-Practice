using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.DTO
{
    public class UserForLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
