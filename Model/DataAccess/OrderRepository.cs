using Dapper;
using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Model.DataAccess
{
    public class OrderRepository : IOrderRepository
    {
        public string ConnectionString = "xd";
        private readonly IOrderElementRepository _orderElementRepository;
        private readonly IOrderSubElementRepository _orderSubElementRepository;
        private readonly OrmLiteConnectionFactory db;

        public OrderRepository(IOrderElementRepository orderElementRepository, IOrderSubElementRepository orderSubElementRepository)
        {
            db = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString, SqliteDialect.Provider);
            _orderElementRepository = orderElementRepository;
            _orderSubElementRepository = orderSubElementRepository;
        }

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

        public void ChangePriority(int orderId, OrderPriority newOrderPriority)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"UPDATE MarioPizzaOrder SET Priority = {(int)newOrderPriority} WHERE OrderId = {orderId}";
                con.Execute(query);
            }

        }

        public void ChangeStatus(MarioPizzaOrder orderWithChangedStatus)
        {
            //dbConn.Update(orderWithChangedStatus);
            //Still doesn't save the data
            //SHIT!
            var dbConn = db.Open();
            using(IDbTransaction dbTrans = dbConn.OpenTransaction())
            {
                dbConn.Save(orderWithChangedStatus);
                dbTrans.Commit();
            }
        }

        public int Count()
        {
            return (int) db.Open().Count<MarioPizzaOrder>();
        }

        public void Edit(MarioPizzaOrder editOne)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(int orderId)
        {
            return db.Open().Exists<MarioPizzaOrder>(x => x.OrderId == orderId);
        }

        public MarioPizzaOrder Get(int orderId)
        {
            return db.Open().SingleById<MarioPizzaOrder>(orderId);
        }

        public List<MarioPizzaOrder> GetAll()
        {
            return db.Open().Select<MarioPizzaOrder>();
            /*
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT " +
                    "M.OrderId,M.ClientPhoneNumber,M.DeliveryAddress," +
                    "M.Priority,M.Status,M.OrderTime " +
                    "FROM MarioPizzaOrder AS M";
                return (List<MarioPizzaOrder>)con.Query<MarioPizzaOrder>(query);
            }
            */
        }

        public List<MarioPizzaOrder> GetByStatus(OrderStatus status)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT " +
                    "OrderId,ClientPhoneNumber,DeliveryAddress,Priority,Status,OrderTime" +
                    $"FROM MarioPizzaOrder WHERE Status = {(int)status}";
                return (List<MarioPizzaOrder>)con.Query<MarioPizzaOrder>(query);
            }
        }

        public void Remove(int orderId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"DELETE FROM MarioPizzaOrder AS M WHERE M.OrderId = {orderId}";
                con.Execute(query);
            }
        }

        public int OrderNextId()
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"SELECT TOP 1 OrderElementId MarioPizzaOrder ORDER BY OrderElementId DESC";
                return con.QueryFirst<int>(query) + 1;
            }
        }

        public double CalculatePriceForOrder(int orderId)
        {
            return 0;
        }
    }
}