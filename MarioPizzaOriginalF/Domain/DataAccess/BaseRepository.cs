using System;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public void Add(T element)
        {
            using (var dbConn = connection.Open())
            {
                dbConn.Insert(element, true);
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
        public bool Exists(int elementId) => connection.Open().SingleById<T>(elementId) != null;
        public bool Exists(Expression<Func<T, bool>> condition) => connection.Open().Single(condition) != null;
        public T Get(int elementId) => connection.Open().SingleById<T>(elementId);
        public T Get(Expression<Func<T, bool>> condition, bool references = false)
        {
            using (var dbConn = connection.Open())
            {
                var unreferenced = dbConn.Single(condition);
                if (references)
                    dbConn.LoadReferences(unreferenced);
                return unreferenced;
            }
        }
        public List<T> GetAll() => connection.Open().Select<T>();

        public List<T> GetAll(Expression<Func<T, bool>> condition, bool references = false)
        {
            using (var dbConn = connection.Open())
            {
                var unreferencedList = dbConn.Select(condition);
                if (references)
                    unreferencedList.ForEach(item => dbConn.LoadReferences(item));
                return unreferencedList;
            }
        }

        public void Remove(int elementId)
        {
            using (var dbConn = connection.Open())
            {
                dbConn.DeleteById<T>(elementId);
            }
        }
        public void Remove(Expression<Func<T, bool>> condition)
        {
            using (var dbConn = connection.Open())
            {
                dbConn.Delete(condition);
            }
        }

        public List<T> Query(string queryString) => connection.Open().Query<T>(queryString).ToList();
    }
}
