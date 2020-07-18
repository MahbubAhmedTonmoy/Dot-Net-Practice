using NoteBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBookAPI.Observer
{
    public interface Subject<T>
    {
        public void Attach(Observer<T> o);
        public void Delete(Observer<T> o);
        public void Notify(T entity);
    }
    public interface Observer<T>
    {
        public void Update(T entity);
    }
}
