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
        private readonly List<Observer<Like>> likeObservers = new List<Observer<Like>>();
        public LikeRepository(ApplicationDataContext db): base(db)
        {
            _db = db;
        }

        public void Attach(Observer<Like> o)
        {
            likeObservers.Add(o);
        }

        public void Delete(Observer<Like> o)
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
