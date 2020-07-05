using SeliseExam.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IDemoRepository DemoRepository { get; private set; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            DemoRepository = new DemoRepository(_db);
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
