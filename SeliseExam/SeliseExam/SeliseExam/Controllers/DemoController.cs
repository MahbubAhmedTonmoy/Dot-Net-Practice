using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeliseExam.Model;
using SeliseExam.pagging;
using SeliseExam.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeliseExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles= "User,SuperAdmin")]
    public class DemoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DemoController> _log;
        public DemoController(IUnitOfWork unitOfWork, ILogger<DemoController> log)
        {
            _log = log;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Demo>> Get(int id)
        {
            var result =await _unitOfWork.DemoRepository.Get(id);
            if(result == null)
            {
                _log.LogInformation($"no Demo exist {id}");
                return BadRequest();
            }
            return result;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Demo>>> GetAll([FromBody]PaggingParms paggingParms)
        {
            //Payload:
            //{
            //      "MinAge": 17,
            //      "MaxAge": 40,
            //      "Gender": "female",
            //      "Orderby": 1,
            //      "PageNumber": 1,
            //      "PageSize": 4
            //}
            var result =await _unitOfWork.DemoRepository.GetAll(paggingParms);
            if (result == null)
            {
                _log.LogInformation($"no Demo exist");
                return BadRequest();
            }
            return result.ToList();
        }

        [HttpPost("create")]
        [Authorize(Policy = "CreatePolicy")]
        public async Task<ActionResult> Create(Demo demo)
        {
            await _unitOfWork.DemoRepository.Create(demo);
            var result = await _unitOfWork.Save();
            if (result == 0)
            {
                _log.LogInformation($"saving problem in DemoCOntroller Create method");
                return BadRequest("saving problem");
            } 
            return Ok("created");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ifExist = _unitOfWork.DemoRepository.Get(id);
            if (ifExist != null)
            {
                await _unitOfWork.DemoRepository.Remove(id);
                var result = await _unitOfWork.Save();
                if (result == 0)
                {
                    _log.LogInformation($"saving problem in DemoCOntroller Delete method");
                    return BadRequest("saving problem");
                }
                return Ok("deleted");
            }
            _log.LogInformation($"no Demo exist {id}");
            return BadRequest();
        }

        [HttpPost("update/{id}")]
        public async Task<ActionResult> Update(int id, Demo demo)
        {
            await _unitOfWork.DemoRepository.Update(id, demo);
            var result = await _unitOfWork.Save();
            if (result == 0)
            {
                _log.LogInformation($"saving problem in DemoCOntroller Update method");
                return BadRequest("saving problem");
            }
            return Ok("updated");
        }

        //https://localhost:5001/api/demo/search?s=ban
        [HttpGet("search")]
        public async Task<ActionResult<List<Demo>>> Search([FromQuery] string s)
        {
            var result = await _unitOfWork.DemoRepository.Search(s);
            if(result != null)
            {
                return result;
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("csv")]
        public IActionResult CsvGenerate()
        {
            var getData = _unitOfWork.DemoRepository.GetAllWithOutPgging().ToList();

            var builder = new StringBuilder();
            builder.AppendLine("Id,Name,Age,Gender");

            foreach (var data in getData)
            {
                builder.AppendLine($"{data.Id},{data.Name},{data.Age},{data.Gender}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "demo.csv");
        }

        [HttpGet("xl")]
        public IActionResult Excel()
        {
            var getData = _unitOfWork.DemoRepository.GetAllWithOutPgging().ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Age";
                worksheet.Cell(currentRow, 4).Value = "Gender";
                foreach (var user in getData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = user.Id;
                    worksheet.Cell(currentRow, 2).Value = user.Name;
                    worksheet.Cell(currentRow, 3).Value = user.Age;
                    worksheet.Cell(currentRow, 4).Value = user.Gender;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }
        }
    }
}
