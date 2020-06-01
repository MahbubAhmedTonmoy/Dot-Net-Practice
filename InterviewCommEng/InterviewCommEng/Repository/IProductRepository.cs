using System.Collections.Generic;
using System.Threading.Tasks;
using InterviewCommEng.Models;

namespace InterviewCommEng.Repository
{
    public interface IProductRepository 
    {
        Product Get(int id);
        Task<IEnumerable<Product>> GetAll();    
        void Add(Product entity);
        void Remove(int id);
        void Remove(Product entity);
        void Update(Product product);
    }
}