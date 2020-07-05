using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogMicroservice.Model;
using CatalogMicroservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CatalogMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {

        private readonly ICatalogRepository _repo;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger, ICatalogRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CatalogItem>> Get()
        {
            var catalogItems = _repo.GetCatalogItems();
            return Ok(catalogItems);
        }

        [HttpGet("{id}")]
        public ActionResult<CatalogItem> Get(Guid id)
        {
            var catalogItem = _repo.GetCatalogItem(id);
            return Ok(catalogItem);
        }

        [HttpPost]
        public ActionResult Post([FromBody] CatalogItem catalogItem)
        {
            _repo.InsertCatalogItem(catalogItem);
            return CreatedAtAction(nameof(Get), new { id = catalogItem.Id }, catalogItem);
        }

        [HttpPut]
        public ActionResult Put([FromBody] CatalogItem catalogItem)
        {
            if (catalogItem != null)
            {
                _repo.UpdateCatalogItem(catalogItem);
                return new OkResult();
            }
            return new NoContentResult();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _repo.DeleteCatalogItem(id);
            return new OkResult();
        }

    }
}
