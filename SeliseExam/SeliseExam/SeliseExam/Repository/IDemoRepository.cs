using SeliseExam.Model;
using SeliseExam.pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.Repository
{
    public interface IDemoRepository: IRepository<Demo>
    {
        Task Update(int id,Demo entity);
        Task<List<Demo>> Search(string s);
        //Task<PagedList<Demo>> GetAll(PaggingParms paggingParms);
    }
}
