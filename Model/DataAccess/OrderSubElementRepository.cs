using ServiceStack.OrmLite;
using System.Collections.Generic;

namespace Model.DataAccess
{
    internal class OrderSubElementRepository : BaseRepository<OrderSubElement>, IOrderSubElementRepository
    {
        private readonly OrmLiteConnectionFactory db;
        public OrderSubElementRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
        }

        public List<OrderSubElement> GetSubElements(int orderElementId)
        {
            return db.Open().Select<OrderSubElement>($"SELECT * FROM OrderSubElement Where OrderElementId = {orderElementId}");
        }
    }
}