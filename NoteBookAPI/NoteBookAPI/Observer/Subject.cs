using NoteBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Observer
{
    public interface Subject
    {
        public void Attach(LikeObserver o);
        public void Delete(LikeObserver o);
        public void Notify(Like like);
    }
    public interface LikeObserver
    {
        public void Update(Like like);
    }
}
