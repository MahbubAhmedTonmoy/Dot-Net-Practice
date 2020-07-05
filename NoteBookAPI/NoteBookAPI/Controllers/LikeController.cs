using Microsoft.AspNetCore.Mvc;
using NoteBookAPI.Model;
using NoteBookAPI.Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("working");
        }
        [HttpPost("like")]
        public async Task<IActionResult> HitLike(Like like)
        {
            var postAvailable = _unitOfWork.PostRepository.Get(like.PostId);
            if(postAvailable != null)
            {
                _unitOfWork.LikeRepository.Create(like);

                postAvailable.TotalLike = postAvailable.TotalLike + 1;
                _unitOfWork.PostRepository.UpdatePost(postAvailable);
                var result =await _unitOfWork.Save();
                if(result == 0) BadRequest("saving problem");
                return Ok($"liked at post {like.PostId}");
            }
            return BadRequest("this post is not available");
        }
    }
}
