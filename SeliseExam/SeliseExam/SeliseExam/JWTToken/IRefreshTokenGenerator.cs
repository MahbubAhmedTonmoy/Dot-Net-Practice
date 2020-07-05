using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.JWTToken
{
    public interface IRefreshTokenGenerator
    {
        public string GenerateRefreshToken();
    }
}
