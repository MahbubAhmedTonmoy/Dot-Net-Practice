using SeliseExam.pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.Repository
{
    public interface IRepository <T> where T: class
    {
        Task<T> Get(int id);
        Task<PagedList<T>> GetAll(PaggingParms paggingParms);
        IEnumerable<T> GetAllWithOutPgging();
        Task Create(T entity);
        Task Remove(int id);
    }
}
