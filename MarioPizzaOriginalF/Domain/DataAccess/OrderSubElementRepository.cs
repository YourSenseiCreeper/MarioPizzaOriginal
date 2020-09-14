using ServiceStack.OrmLite;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class OrderSubElementRepository : BaseRepository<OrderSubElement>, IOrderSubElementRepository
    {
        public OrderSubElementRepository()
        {
            using (var conn = connection.Open())
            {
                if (conn.CreateTableIfNotExists<OrderSubElement>())
                {
                    conn.Insert(
                        new OrderSubElement
                        {
                            OrderSubElementId = 1,
                            OrderElementId = 1,
                            FoodId = 1,
                            Amount = 1
                        });
                }
            }
        }

        public List<OrderSubElement> GetSubElements(int orderElementId)
        {
            return connection.Open().Select<OrderSubElement>($"SELECT * FROM OrderSubElement Where OrderElementId = {orderElementId}");
        }
    }
}