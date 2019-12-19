using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Model.DataAccess
{
    internal class OrderElementRepository : IOrderElementRepository
    {
        public string ConnectionString => ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString;
        private readonly IOrderSubElementRepository _orderSubElementRepository;

        public OrderElementRepository(IOrderSubElementRepository orderSubElementRepository)
        {
            _orderSubElementRepository = orderSubElementRepository;
        }

        public void Add(OrderElement newOne)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var output = con.Execute(
                    "INSERT INTO MarioPizzaOrderElement (" +
                    newOne.OrderElementId != null ? "OrderElementId," : "" + 
                    "OrderId, FoodId, Amount) " +
                    $"VALUES (" +
                    newOne.OrderElementId != null ? $"{newOne.OrderElementId}," : "" +
                    $"{newOne.OrderId},{newOne.FoodId},{newOne.Amount})");
            }
        }

        public void AddToOrder(int orderId, int foodId, double quantity)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var output = con.Execute(
                    "INSERT INTO MarioPizzaOrderElement (OrderId,FoodId,Amount) " +
                    $"VALUES ({orderId},{foodId},{quantity})");
            }
        }

        public int Count()
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT COUNT(*) MarioPizzaOrderElement";
                return con.ExecuteScalar<int>(query);
            }
        }

        public void Edit(OrderElement editOne)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(int orderElementId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"SELECT COUNT(*) MarioPizzaOrderElement WHERE OrderElementId = {orderElementId}";
                return con.ExecuteScalar<int>(query) > 0;
            }
        }

        public OrderElement Get(int orderId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var output = con.QueryFirst<OrderElement>(
                    $"SELECT OrderElementId,FoodId,Amount FROM MarioPizzaOrderElement WHERE OrderId = {orderId}");
                //SubOrderElements
                output.SubOrderElements = _orderSubElementRepository.GetSubElements(output.OrderId, output.OrderElementId);
                return output;
            }
        }

        public List<OrderElement> GetAll()
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var output = con.Query<OrderElement>(
                    $"SELECT M.OrderElementId,M.FoodId,M.Amount FROM MarioPizzaOrderElement AS M");
                return (List<OrderElement>)output;
            }
        }

        public List<OrderElement> GetElements(int orderId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var output = con.Query<OrderElement>(
                    $"SELECT OrderElementId,FoodId,Amount FROM MarioPizzaOrderElement WHERE OrderId = {orderId}");
                //SubOrderElements
                foreach (var row in output)
                {
                    row.SubOrderElements = _orderSubElementRepository.GetSubElements(row.OrderId, row.OrderElementId);
                }
                return (List<OrderElement>) output;
            }
        }

        public int OrderElementNextId()
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT TOP 1 OrderElementId FROM MarioPizzaOrderElement ORDER BY OrderElementId DESC";
                return con.ExecuteScalar<int>(query) + 1;
            }
        }

        public void Remove(int orderElementId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"DELETE FROM MarioPizzaOrderElement WHERE OrderElementId = {orderElementId}";
                con.Execute(query);
            }
        }

        public void RemoveFromOrder(int orderId, int foodId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"DELETE FROM MarioPizzaOrderElement WHERE OrderId = {orderId} AND FoodId = {foodId}";
                con.Execute(query);
            }
        }
    }
}