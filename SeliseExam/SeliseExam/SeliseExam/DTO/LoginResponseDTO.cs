using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.DTO
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
       // public DateTimeOffset AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
