using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository.IRepo
{
    public interface IRepository <T> where T: class
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void Create(T entity);
        Task Remove(int id);
    }
}
