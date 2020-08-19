using ServiceStack.OrmLite;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class OrderSubElementRepository : BaseRepository<OrderSubElement>, IOrderSubElementRepository
    {
        private readonly OrmLiteConnectionFactory db;
        public OrderSubElementRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
            using (var conn = dbConnection.Open())
            {
                if (conn.CreateTableIfNotExists<OrderSubElement>())
                {
                    conn.Insert(
                        new OrderSubElement
                        {
                            SubOrderElementId = 1,
                            OrderElementId = 1,
                            FoodId = 1,
                            Amount = 1
                        });
                }
            }
        }

        public List<OrderSubElement> GetSubElements(int orderElementId)
        {
            return db.Open().Select<OrderSubElement>($"SELECT * FROM OrderSubElement Where OrderElementId = {orderElementId}");
        }
    }
}