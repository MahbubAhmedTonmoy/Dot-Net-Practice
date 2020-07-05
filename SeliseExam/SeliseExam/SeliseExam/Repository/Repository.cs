using Microsoft.EntityFrameworkCore;
using SeliseExam.Data;
using SeliseExam.pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbset;
        public Repository(AppDbContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();

        }
        public async Task Create(T entity)
        {
            await dbset.AddAsync(entity);
        }

        public async Task<T> Get(int id)
        {
            return await dbset.FindAsync(id);
        }

        public Task<PagedList<T>> GetAll(PaggingParms paggingParms)
        {
            var getData = dbset.AsQueryable();
            return  PagedList<T>.CreteAsync(getData, paggingParms.PageNumber, paggingParms.PageSize);
        }

        public IEnumerable<T> GetAllWithOutPgging()
        {
            return dbset.ToList();
        }

        public async Task Remove(int id)
        {
            var getEntity = await dbset.FindAsync(id);
            dbset.Remove(getEntity);
        }

    }
}
