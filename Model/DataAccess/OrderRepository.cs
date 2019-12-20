using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using ServiceStack.OrmLite;
using System.Collections.Generic;

namespace Model.DataAccess
{
    public class OrderRepository : BaseRepository<MarioPizzaOrder>, IOrderRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public OrderRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
        }
        /*
        public void Add(MarioPizzaOrder order)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "INSERT INTO MarioPizzaOrder (OrderId,ClientPhoneNumber,DeliveryAddress,Priority,Status,OrderTime) " +
                    $"VALUES ({order.OrderId},'{order.ClientPhoneNumber}','{order.DeliveryAddress}',{(int)order.Priority},{(int)order.Status},'{order.OrderTime.ToString("dd/MM/yyyy HH:MM:ss")}')";
                var output = con.Execute(query);
                //Adding OrderElements and SubOrderElements
                List<OrderElement> orderElements = new List<OrderElement>();
                orderElements.ForEach(orderElement =>
                {
                    _orderElementRepository.Add(new OrderElement
                    {
                        OrderId = order.OrderId,
                        FoodId = orderElement.FoodId,
                        Amount = orderElement.Amount
                    });
                    //AddElementToOrder(order.OrderId, orderElement.FoodId, orderElement.Amount);
                    if (orderElement.SubOrderElements != null)
                    {
                        orderElement.SubOrderElements.ForEach(subOrder => _orderSubElementRepository.Add(subOrder));
                    }
                });
            }
        }
        */
        public void ChangePriority(int orderId, OrderPriority newOrderPriority)
        {
            db.Open().Save(newOrderPriority);
        }

        public void ChangeStatus(MarioPizzaOrder orderWithChangedStatus)
        {
            db.Open().Save(orderWithChangedStatus);
        }

        public List<MarioPizzaOrder> GetByStatus(OrderStatus status)
        {
            return db.Open().Select<MarioPizzaOrder>(x => x.Status == status);
        }

        public int OrderNextId()
        {
            return db.Open().Scalar<MarioPizzaOrder, int>(x => Sql.Max(x.OrderId)) + 1;
        }

        public double CalculatePriceForOrder(int orderId)
        {
            return 0;
        }
    }
}