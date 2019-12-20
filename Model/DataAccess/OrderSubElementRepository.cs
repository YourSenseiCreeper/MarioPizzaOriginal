using Dapper;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Model.DataAccess
{
    internal class OrderSubElementRepository : BaseRepository<OrderSubElement>, IOrderSubElementRepository
    {
        private readonly OrmLiteConnectionFactory db;
        public OrderSubElementRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
        }

        public List<OrderSubElement> GetSubElements(int orderId, int orderElementId)
        {
            return db.Open().Select<OrderSubElement>(x => x.OrderId == orderId && x.OrderElementId == orderElementId);
        }
    }
}