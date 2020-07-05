using NoteBookAPI.Data;
using NoteBookAPI.Model;
using NoteBookAPI.Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Repository
{
    public class LikeRepository :Repository<Like>, ILikeRepository
    {

        private readonly ApplicationDataContext _db;
        public LikeRepository(ApplicationDataContext db): base(db)
        {
            _db = db;
        }

        //public void Add(Like like)
        //{
        //    _db.Like.Add(like);
        //}
    }
}
