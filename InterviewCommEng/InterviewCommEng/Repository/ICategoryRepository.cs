using System.Collections.Generic;
using System.Threading.Tasks;
using InterviewCommEng.Models;

namespace InterviewCommEng.Repository
{
    public interface ICategoryRepository
    {
        Category Get(int id);
        Task<IEnumerable<Category>> GetAll();    
        void Add(Category entity);
        void Remove(int id);
        void Remove(Category entity);
        void Update(Category category);
    }
}