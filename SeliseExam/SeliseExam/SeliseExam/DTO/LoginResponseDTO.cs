using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.DTO
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string expires_in { get; set; }
        public string RefreshToken { get; set; }
    }
}
