using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.Repository
{
    interface IUnitOfWork: IDisposable
    {
        IPostRepository PostRepository { get; }
        
        Task<int> Save();
    }
}
