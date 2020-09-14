using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Data;
using TinyIoC;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class BaseRepository<T> : IRepository<T>
    {
        protected readonly IDbConnectionFactory connection;

        public BaseRepository()
        {
            connection = TinyIoCContainer.Current.Resolve<IDbConnectionFactory>();
        }

        public T GetWithReferences(int id)
        {
            using (var dbConn = connection.Open())
            {
                var element = dbConn.SingleById<T>(id);
                dbConn.LoadReferences(element);
                return element;
            }
        }

        public void Add(T element)
        {
            using (var dbConn = connection.Open())
            {
                dbConn.Save(element, true);
            }
        }
        public int Count() => (int) connection.Open().Count<T>();

        public void Save(T editedElement)
        {
            using (var dbConn = connection.Open())
            {
                dbConn.Save(editedElement, true);
            }
        }
        public bool Exists(int elementId) => Get(elementId) != null;
        public T Get(int elementId) => connection.Open().SingleById<T>(elementId);
        public List<T> GetAll() => connection.Open().Select<T>();
        public void Remove(int elementId)
        {
            using (var dbConn = connection.Open())
            {
                dbConn.DeleteById<T>(elementId);
            }
        }

        public List<T> Query(string queryString) => connection.Open().Query<T>(queryString).ToList();
    }
}
