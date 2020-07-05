using FBLite.Data;
using FBLite.DTOs;
using FBLite.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FBLite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;
        private readonly SignInManager<AppUser> _signInManager;
        public PostController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, AppDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _db = db;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(Post post)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId == null) { return Unauthorized(); }
            var postToCreate = new Post
            {
                UserId = userId,
                Description = post.Description,
                PostDate = DateTime.Now
            };
            _db.Posts.Add(postToCreate);
            var result = await _db.SaveChangesAsync();
            if (result > 0)
            { return Ok(); }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<PostViewDTO>>> GetAll()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var checkRole = User.Claims.FirstOrDefault((c => c.Type == ClaimTypes.Role)).Value;

            if(checkRole == Role.SuperAdmin)
            {
                var result =await _db.Posts.Include(i => i.Comments).Include(l => l.Likes).Include(u => u.User).ToListAsync();

                List<PostViewDTO> temp = new List<PostViewDTO>();
                foreach(var i in result)
                {
                    temp.Add(new PostViewDTO
                    {
                        Description = i.Description,
                        PostedBy = i.User.Email,
                        PostedDate = i.PostDate,
                        TotalComment = i.Comments.Select(x => x.Id == i.Id).Count(),
                        TotalLike = i.Likes.Select(x => x.id == i.Id).Count(),
                        Comment = new CommentViewDTO { Description = i.Comments.Select(x => x.Description).ToList()}
                    });

                }

                return temp;
            }
            return Unauthorized();
        }

        //https://localhost:5001/api/post/search?s=ban
        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<List<Post>>> Search([FromQuery]string s)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var checkRole = User.Claims.FirstOrDefault((c => c.Type == ClaimTypes.Role)).Value;
            if(checkRole == Role.User || checkRole == Role.SuperAdmin)
            {
                if (userId == null) { return Unauthorized(); }

                var result = await _db.Posts.Where(i => i.Description.Contains(s))
                                .Include(c => c.Comments).Include(x => x.Likes).Include(u => u.User).ToListAsync();
               
                return result;
            }
            return Unauthorized();
            
        }
    }
}
