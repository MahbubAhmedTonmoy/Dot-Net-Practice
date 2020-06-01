using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewCommEng.Data;
using InterviewCommEng.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewCommEng.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _db;
        public CategoryRepository(DataContext db)
        {
            _db = db;
        }

        public void Add(Category entity)
        {
            _db.Categories.Add(entity);
        }

        public Category Get(int id)
        {
            return _db.Categories.Find(id);
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var a = await _db.Categories.ToListAsync();
            return a;
        }

        public void Remove(int id)
        {
            var entity = _db.Categories.Find(id);
            Remove(entity);
        }

        public void Remove(Category entity)
        {
            _db.Categories.Remove(entity);
        }

        public void Update(Category category)
        {
            var getCategory = _db.Categories.FirstOrDefault(c => c.Id == category.Id);

            getCategory.Name = category.Name;
        }
    }
}