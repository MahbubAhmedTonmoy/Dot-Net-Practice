using System.Threading.Tasks;
using InterviewCommEng.Data;

namespace InterviewCommEng.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _db;
        public IProductRepository ProductRepository {get; private set;}
        public ICategoryRepository CategoryRepository {get; private set;}
        public UnitOfWork(DataContext db)
        {
            _db = db;
            ProductRepository = new ProductRepository(_db);
            CategoryRepository = new CategoryRepository(_db);
        }
        

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<int> Save()
        {
            var result = await _db.SaveChangesAsync();
            return result;
        }
    }
}