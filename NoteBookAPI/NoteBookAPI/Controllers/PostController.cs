using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class PostController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _log;
        public PostController(IUnitOfWork unitOfWork, ILogger<PostController> log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }
        [HttpGet("test")]
        public  IActionResult Test()
        {
            _log.LogInformation("enter test api");
            return Ok("working");
        }

        [HttpGet("post/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = _unitOfWork.PostRepository.Get(id);
            if(result != null){
                return Ok(result);
            }
            return BadRequest($"{id} no post available");
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result =  _unitOfWork.PostRepository.GetAll();
            if(result != null){
                return Ok(result);
            }
            return BadRequest($" no post available");
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(Post post)
        {
            post.TotalLike = 0;
            post.TotalComment = 0;
            _unitOfWork.PostRepository.Create(post);
            var result = await _unitOfWork.Save();
            if(result == 0) return BadRequest("saving problem");
            return Ok("created");
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Update(Post post)
        {
            _unitOfWork.PostRepository.UpdatePost(post);
            var result = await _unitOfWork.Save();
            if(result == 0) return BadRequest("saving problem");
            return Ok("created");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitOfWork.PostRepository.Remove(id);
            var result =await _unitOfWork.Save();
            if (result == 0) return BadRequest("saving problem");
            return Ok("deleted");
        }

        //https://localhost:5001/api/post/search?s=ban
        [HttpGet("search")]
        public IActionResult Seach([FromQuery]string s)
        {
            var result = _unitOfWork.PostRepository.Search(s);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest("no status found");
        }
       
    }
}
