using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.Model
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
