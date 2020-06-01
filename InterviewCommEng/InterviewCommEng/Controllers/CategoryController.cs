using System.Threading.Tasks;
using InterviewCommEng.Models;
using InterviewCommEng.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InterviewCommEng.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("getcategory/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = _unitOfWork.CategoryRepository.Get(id);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest($"not item in DB with id: {id}");
            
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result =await _unitOfWork.CategoryRepository.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest($"not item in DB");
            
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]Category entity)
        {
            _unitOfWork.CategoryRepository.Add(entity);
            var result = await _unitOfWork.Save();
            if(result == 0) return BadRequest();
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]Category entity)
        {
            _unitOfWork.CategoryRepository.Update(entity);
            var result = await _unitOfWork.Save();
            if (result == 0) return BadRequest("saving problem");
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var getFromDB = _unitOfWork.CategoryRepository.Get(id);
            if (getFromDB == null)
            {
                return BadRequest("no item in DB");
            }
            _unitOfWork.CategoryRepository.Remove(getFromDB);
            var result = await _unitOfWork.Save();
            if (result == 0) return BadRequest("saving problem");
            return Ok();
        }


    }
}