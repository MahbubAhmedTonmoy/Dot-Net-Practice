using NoteBookAPI.Data;
using NoteBookAPI.Model;
using NoteBookAPI.Observer;
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
        private readonly List<LikeObserver> likeObservers = new List<LikeObserver>();
        public LikeRepository(ApplicationDataContext db): base(db)
        {
            _db = db;
        }

        public void Attach(LikeObserver o)
        {
            likeObservers.Add(o);
        }

        public void Delete(LikeObserver o)
        {
            var findObserver = likeObservers.IndexOf(o);
            likeObservers.RemoveAt(findObserver);
        }

        public void Notify(Like like)
        {
            foreach(var o in likeObservers)
            {
                o.Update(like);
            }
        }

        //public void Add(Like like)
        //{
        //    _db.Like.Add(like);
        //}
    }
}
