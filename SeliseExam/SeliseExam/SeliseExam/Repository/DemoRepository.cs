using Microsoft.EntityFrameworkCore;
using SeliseExam.Data;
using SeliseExam.Model;
using SeliseExam.pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.Repository
{
    public class DemoRepository : Repository<Demo>, IDemoRepository
    {
        private readonly AppDbContext _db;
        public DemoRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Demo>> Search(string s)
        {
            var getDemo = await _db.Demo.Where(x => x.Name.Contains(s)).ToListAsync();
            return getDemo;
        }

        public async Task Update(int id,Demo entity)
        {
            var getEntity = await _db.Demo.FirstOrDefaultAsync(i => i.Id == id);
            if(getEntity != null)
            {
                getEntity.Name = entity.Name;
                getEntity.Age = entity.Age;
                getEntity.Gender = entity.Gender;
            }
        }

        //ovar ride the GetALL mathod of Repository
        public async Task<PagedList<Demo>> GetAll(PaggingParms paggingParms)
        {
            var getData = dbset.AsQueryable();
            getData = getData.Where(x => x.Gender == paggingParms.Gender);
            getData = getData.Where(x => x.Age >= paggingParms.MinAge && x.Age <= paggingParms.MaxAge);
            if (paggingParms.OrderBy == 1)
            {
                getData = getData.OrderBy(x => x.Age);
            }
            else
            {
                getData = getData.OrderByDescending(x => x.Age);
            }
            return await PagedList<Demo>.CreteAsync(getData, paggingParms.PageNumber, paggingParms.PageSize);
        }
    }
}
