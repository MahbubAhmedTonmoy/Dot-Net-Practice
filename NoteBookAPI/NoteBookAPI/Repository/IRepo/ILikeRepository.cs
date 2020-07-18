using NoteBookAPI.Model;
using NoteBookAPI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository.IRepo
{
    public interface ILikeRepository: IRepository<Like>, Subject<Like>
    {
        //void Add(Like like);
    }
}
