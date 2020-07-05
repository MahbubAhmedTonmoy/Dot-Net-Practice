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
    public class CommentController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("working");
        }

        [HttpPost("comment")]
        public async Task<IActionResult> Comment(Comment comment)
        {
            var postAvailable = _unitOfWork.PostRepository.Get(comment.PostId);
            if(postAvailable != null)
            {
                _unitOfWork.commentRepository.Create(comment);

                postAvailable.TotalComment = postAvailable.TotalComment + 1;
                _unitOfWork.PostRepository.UpdatePost(postAvailable);
                var result =await _unitOfWork.Save();
                if(result == 0) BadRequest("saving problem");
                return Ok($"commented at post {comment.PostId}");
            }
            return BadRequest("this post is not available");
        }
    }
}
