using Microsoft.Extensions.Logging;
using NoteBookAPI.Data;
using NoteBookAPI.Observer;
using NoteBookAPI.Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDataContext _db;
        
             
        public IPostRepository PostRepository { get; private set; }
        private readonly ILogger<PostRepository> _log;
        public ILikeRepository LikeRepository { get; private set; }
        public LikeObserver _LikeObserver;
        public ICommentRepository commentRepository { get; private set; }
        public UnitOfWork(ApplicationDataContext db, ILogger<PostRepository> log)
        {
            _db = db;
            _log = log;
            PostRepository = new PostRepository(_db,_log);
            LikeRepository = new LikeRepository(_db);
            commentRepository = new CommentRepository(_db);
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
