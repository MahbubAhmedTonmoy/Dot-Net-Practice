using System;
using System.Threading.Tasks;

namespace InterviewCommEng.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IProductRepository ProductRepository {get;}
        ICategoryRepository CategoryRepository {get;}

        Task<int> Save();
    }
}