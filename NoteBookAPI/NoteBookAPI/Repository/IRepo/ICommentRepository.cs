using NoteBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository.IRepo
{
    public interface ICommentRepository: IRepository<Comment>
    {
        //void Add(Comment comment);
    }
}
