using FBLite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IPostRepository PostRepository { get; private set; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            PostRepository = new PostRepository(_db);
        }
        

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<int> Save()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
