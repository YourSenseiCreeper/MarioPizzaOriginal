using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Linq;

namespace Model.DataAccess
{
    public class BaseRepository<T> : IRepository<T>
    {
        private readonly OrmLiteConnectionFactory db;

        public BaseRepository(OrmLiteConnectionFactory dbConnection) => db = dbConnection;

        public void Add(T element) => db.Open().Insert(element);
        public int Count() => (int) db.Open().Count<T>();
        public void Edit(T editedElement) => db.Open().Save(editedElement);
        public bool Exists(int elementId) => Get(elementId) != null;
        public T Get(int elementId) => db.Open().SingleById<T>(elementId);
        public List<T> GetAll() => db.Open().Select<T>();
        public void Remove(int elementId) => db.Open().DeleteById<T>(elementId);
    }
}
