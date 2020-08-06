using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;

namespace Model.DataAccess
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public OrderRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
            using (var conn = dbConnection.Open())
            {
                if (conn.CreateTableIfNotExists<Order>())
                {
                    conn.Insert(
                        new Order
                        {
                            OrderId = 1,
                            ClientPhoneNumber = "133244355",
                            DeliveryAddress = "Radolin 5",
                            Priority = OrderPriority.NORMAL,
                            Status = OrderStatus.WAITING,
                            PaymentMethod = Enums.PaymentMethod.CASH,
                            Payment = Enums.Payment.AT_PLACE,
                            OrderTime = DateTime.Now
                        });
                }
            }
        }

        public List<Order> GetByStatus(OrderStatus status)
        {
            return db.Open().Select<Order>(x => x.Status == status);
        }

        public int OrderNextId()
        {
            return db.Open().Scalar<Order, int>(x => Sql.Max(x.OrderId)) + 1;
        }

        public double CalculatePriceForOrder(int orderId)
        {
            var sql = $@"SELECT
                SUM((F.Price * O.Amount)
                IFNULL((SELECT SUM((F2.Price * OE.Amount)) FROM OrderSubElement AS OE 
                INNER JOIN OrderElement O2 ON O2.OrderElementId = OE.OrderElementId
                LEFT JOIN Food F2 ON F2.FoodId = OE.FoodId
                WHERE O2.OrderElementId = O.OrderElementId), 0)
                ) AS Cena
                FROM Food F
                JOIN OrderElement O ON O.FoodId = F.FoodId
                WHERE O.OrderId = {orderId}";
            return db.Open().Single<double>(sql);
        }
    }
}