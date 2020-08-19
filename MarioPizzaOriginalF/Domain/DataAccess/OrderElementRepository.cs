using ServiceStack.OrmLite;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class OrderElementRepository : BaseRepository<OrderElement>, IOrderElementRepository
    {
        private readonly OrmLiteConnectionFactory db;
        public OrderElementRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
            using (var conn = dbConnection.Open())
            {
                if (conn.CreateTableIfNotExists<OrderElement>())
                {
                    conn.Insert(
                        new OrderElement
                        {
                            OrderElementId = 1,
                            OrderId = 1,
                            FoodId = 1,
                            Amount = 1
                        });
                }
            }
        }

        public void AddToOrder(int orderId, int foodId, double amount)
        {
            db.Open().Insert(new OrderElement
            {
                OrderId = orderId,
                FoodId = foodId,
                Amount = amount
            });
        }

        public List<OrderElement> GetElements(int orderId)
        {
            return db.Open().Select<OrderElement>($"SELECT * FROM OrderElement WHERE OrderId = {orderId}");
        }

        public int OrderElementNextId()
        {
            return db.Open().Scalar<OrderElement, int>(x => Sql.Max(x.OrderId)) + 1;
        }

        public void RemoveFromOrder(int orderId, int foodId)
        {
            db.Open().Delete<OrderElement>(x => x.OrderId == orderId && x.FoodId == foodId);
        }

        public bool IsElementInOrder(int orderId, int orderElementId)
        {
            return db.Open().Exists<OrderElement>(x => x.OrderId == orderId && x.OrderElementId == orderElementId);
        }
    }
}