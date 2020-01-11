using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccess
{
    public class BaseRepository<T> : IRepository<T>
    {
        private readonly OrmLiteConnectionFactory db;

        public BaseRepository(OrmLiteConnectionFactory dbConnection)
        {
            db = dbConnection;
        }

        public void Add(T element)
        {
            db.Open().Insert(element);
        }

        public int Count()
        {
            return (int) db.Open().Count<T>();
        }

        public void Edit(T editedElement)
        {
            db.Open().Save(editedElement);
            //db.Open().Save<T>(editedElement);
        }

        public bool Exists(int elementId)
        {
            return Get(elementId) != null;
        }

        public T Get(int elementId)
        {
            return db.Open().SingleById<T>(elementId);
        }

        public List<T> GetAll()
        {
            return db.Open().Select<T>();
        }

        public void Remove(int elementId)
        {
            db.Open().DeleteById<T>(elementId);
        }
    }
}
