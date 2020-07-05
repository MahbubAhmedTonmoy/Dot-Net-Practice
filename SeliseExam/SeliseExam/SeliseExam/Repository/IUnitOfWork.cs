using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IDemoRepository DemoRepository { get; }
        Task<int> Save();
    }
}
