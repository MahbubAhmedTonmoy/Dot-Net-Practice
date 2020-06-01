using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewCommEng.Data;
using InterviewCommEng.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewCommEng.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _db;

        public ProductRepository(DataContext db)
        {
            _db = db;
        }
        public void Add(Product entity)
        {
            _db.Products.Add(entity);
        }

        public Product Get(int id)
        {
            return _db.Products.Include(c => c.Category).FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var a = await _db.Products.Include(x => x.Category).ToListAsync();
            return a;
        }

        public void Remove(int id)
        {
            var entity = _db.Products.Find(id);
            Remove(entity);
        }

        public void Remove(Product entity)
        {
            _db.Products.Remove(entity);
        }
        public void Update(Product product)
        {
            var getProduct = _db.Products.FirstOrDefault(p => p.Id == product.Id);

            getProduct.Name = product.Name;
            getProduct.Category = product.Category;
            getProduct.CreateDate = product.CreateDate;

           // _db.SaveChanges();
        }
    }
}