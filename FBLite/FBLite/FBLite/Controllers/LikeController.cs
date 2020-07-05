using FBLite.Data;
using FBLite.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FBLite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;
        private readonly SignInManager<AppUser> _signInManager;
        public LikeController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, AppDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _db = db;
        }

        [HttpPost("like")]
        [Authorize]
        public async Task<IActionResult> Like([FromBody]Like like)
        {
            var userFindOut = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if(userFindOut != null)
            {
                var hitLike = new Like
                {
                    LikerId = userFindOut,
                    PostId = like.PostId,
                    CommentId = like.CommentId
                };
                _db.Likes.Add(hitLike);
                var result =await _db.SaveChangesAsync() > 0;
                if (result) return Ok();
                else
                {
                    return BadRequest();
                }

            }
            return BadRequest($"not found");
        }
    }
}
