﻿using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain.DataAccess
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
                            PaymentMethod = PaymentMethod.CASH,
                            Payment = Payment.AT_PLACE,
                            OrderTime = DateTime.Now
                        });
                }
            }
        }

        public new void Add(Order order)
        {
            using (var dbConn = db.Open())
            {
                dbConn.Save(order);
                if (order.OrderElements.Count != 0)
                {
                    dbConn.SaveReferences(order, order.OrderElements);
                    foreach (var orderElement in order.OrderElements)
                    {
                        if (orderElement.SubOrderElements != null)
                            dbConn.SaveReferences(orderElement, orderElement.SubOrderElements);
                    }
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
                        SUM(F.Price * O.Amount) +
                        IFNULL((SELECT SUM(F2.Price * OE.Amount) 
                                FROM OrderSubElement AS OSE 
                                INNER JOIN OrderElement OE ON OE.OrderElementId = OSE.OrderElementId
                                LEFT JOIN Food F2 ON F2.FoodId = OSE.FoodId
                                WHERE OE.OrderElementId = O.OrderElementId), 0) AS Cena
                        FROM Food F
                        JOIN OrderElement O ON O.FoodId = F.FoodId
                        WHERE O.OrderId = {orderId}";
            return db.Open().Single<double>(sql);
        }
    }
}