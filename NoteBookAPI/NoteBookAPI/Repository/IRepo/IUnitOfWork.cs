using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository.IRepo
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        ILikeRepository LikeRepository { get; }
        ICommentRepository commentRepository { get; }

        Task<int> Save();
    }
}
