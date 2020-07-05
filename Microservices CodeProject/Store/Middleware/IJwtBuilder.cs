using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware
{
    public interface IJwtBuilder
    {
        string GetToken(Guid userId);
        Guid ValidateToken(string token);
    }
}
