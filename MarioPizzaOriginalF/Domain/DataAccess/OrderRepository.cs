using MarioPizzaOriginal.Domain.Enums;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public double CalculatePriceForOrder(int orderId)
        {
            var selectedOrder = GetOrderWithAllElements(orderId);
            if (selectedOrder == null)
                return 0;
            
            var price = 0d;
            foreach (var orderElement in selectedOrder.OrderElements)
            {
                price += orderElement.Amount * orderElement.Food.Price;
                if (orderElement.SubOrderElements != null)
                {
                    price += orderElement.SubOrderElements.Sum(subOrderElement => subOrderElement.Amount * subOrderElement.Food.Price);
                }
            }
            return price;
        }

        public Order GetOrderWithAllElements(int orderId)
        {
            using (var dbConn = db.Open())
            {
                var selectedOrder = dbConn.SingleById<Order>(orderId);
                if (selectedOrder == null)
                    return new Order();

                dbConn.LoadReferences(selectedOrder);
                foreach (var orderElement in selectedOrder.OrderElements)
                {
                    dbConn.LoadReferences(orderElement);
                    foreach (var subOrderElement in orderElement.SubOrderElements)
                    {
                        dbConn.LoadReferences(subOrderElement);
                    }
                }
                return selectedOrder;
            }
        }

        public void DeleteOrderWithAllElements(int orderId)
        {
            using (var dbConn = db.Open())
            {
                var selectedOrder = GetOrderWithAllElements(orderId);
                if (selectedOrder == null)
                    return;

                foreach (var orderElement in selectedOrder.OrderElements)
                {
                    if (orderElement.SubOrderElements == null)
                        continue;

                    foreach (var orderSubElement in orderElement.SubOrderElements)
                    {
                        dbConn.Delete(orderSubElement);
                    }

                    dbConn.Delete(orderElement);
                }
                dbConn.Delete(selectedOrder);
            }
        }
    }
}