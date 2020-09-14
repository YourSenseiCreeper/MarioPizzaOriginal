using ServiceStack.OrmLite;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class OrderElementRepository : BaseRepository<OrderElement>, IOrderElementRepository
    {
        public OrderElementRepository()
        {
            using (var conn = connection.Open())
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
            connection.Open().Insert(new OrderElement
            {
                OrderId = orderId,
                FoodId = foodId,
                Amount = amount
            });
        }

        public List<OrderElement> GetElements(int orderId)
        {
            return connection.Open().Select<OrderElement>($"SELECT * FROM OrderElement WHERE OrderId = {orderId}");
        }

        public void RemoveFromOrder(int orderId, int foodId)
        {
            connection.Open().Delete<OrderElement>(x => x.OrderId == orderId && x.FoodId == foodId);
        }

        public bool IsElementInOrder(int orderId, int orderElementId)
        {
            return connection.Open().Exists<OrderElement>(x => x.OrderId == orderId && x.OrderElementId == orderElementId);
        }
    }
}