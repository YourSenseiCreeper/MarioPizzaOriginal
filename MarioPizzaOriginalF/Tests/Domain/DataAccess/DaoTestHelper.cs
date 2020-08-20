using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using ServiceStack.OrmLite;

namespace MarioPizzaOriginal.Tests.Domain.DataAccess
{
    public class DaoTestHelper<TRepository> where TRepository : class
    {
        private readonly OrmLiteConnectionFactory _db;
        private readonly TRepository _repository;

        public DaoTestHelper(Func<OrmLiteConnectionFactory, TRepository> begin)
        {
            var currentPath = Directory.GetCurrentDirectory();
            _db = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["TestSqlLite"].ConnectionString, SqliteDialect.Provider);
            _repository = begin(_db);
        }

        protected void RunInRollbackTransaction(Action<TRepository, IDbTransaction> action)
        {
            using (var dbConn = _db.Open())
            {
                using (var transaction = dbConn.BeginTransaction())
                {
                    action(_repository, transaction);
                    transaction.Rollback();
                }
            }
        }
    }
}
