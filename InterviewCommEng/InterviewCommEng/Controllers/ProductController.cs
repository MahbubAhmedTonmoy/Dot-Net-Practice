using System.Threading.Tasks;
using InterviewCommEng.Models;
using InterviewCommEng.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InterviewCommEng.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("getproduct/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = _unitOfWork.ProductRepository.Get(id);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest($"not item in DB with id: {id}");
            
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result =await _unitOfWork.ProductRepository.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest($"not item in DB");
            
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]Product entity)
        {
            _unitOfWork.ProductRepository.Add(entity);
            var result = await _unitOfWork.Save();
            if(result == 0) return BadRequest();
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]Product entity)
        {
            _unitOfWork.ProductRepository.Update(entity);
            var result = await _unitOfWork.Save();
            if (result == 0) return BadRequest("saving problem");
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var getFromDB = _unitOfWork.ProductRepository.Get(id);
            if (getFromDB == null)
            {
                return BadRequest("no item in DB");
            }
            _unitOfWork.ProductRepository.Remove(getFromDB);
            var result = await _unitOfWork.Save();
            if (result == 0) return BadRequest("saving problem");
            return Ok();
        }


    }
}