using NoteBookAPI.Data;
using NoteBookAPI.Model;
using NoteBookAPI.Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository
{
    public class CommentRepository :Repository<Comment>, ICommentRepository
    {

        private readonly ApplicationDataContext _db;
        public CommentRepository(ApplicationDataContext db): base(db)
        {
            _db = db;
        }

        //public void Add(Comment comment)
        //{
        //    _db.Comment.Add(comment);
        //}
    }
}
