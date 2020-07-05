using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoteBookAPI.Data;
using NoteBookAPI.Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly ApplicationDataContext _db;
        internal DbSet<T> dbset;
        public Repository(ApplicationDataContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }
        public void Create(T entity)
        {
            dbset.Add(entity);
        }

        public T Get(int id)
        {
            return dbset.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return dbset.ToList();
        }

        public async Task Remove(int id)
        {
            var getentity = await dbset.FindAsync(id);
            dbset.Remove(getentity);
        }

    }
}
