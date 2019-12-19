using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Model.DataAccess
{
    internal class OrderSubElementRepository : IOrderSubElementRepository
    {
        public string ConnectionString => ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString;

        public void Add(SubOrderElement subOrder)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "INSERT INTO MarioPizzaOrderSubElement (OrderElementId,FoodId,Amount) " +
                    $"VALUES ({subOrder.OrderElementId},{subOrder.FoodId},{subOrder.Amount})";
                var output = con.Execute(query);
            }
        }

        public int Count()
        {
            throw new System.NotImplementedException();
        }

        public void Edit(SubOrderElement editOne)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new System.NotImplementedException();
        }

        public SubOrderElement Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<SubOrderElement> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public List<SubOrderElement> GetSubElements(int orderId, int orderElementId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var output = con.Query<SubOrderElement>(
                    $"SELECT OrderElementId,FoodId,Amount " +
                    $"FROM MarioPizzaOrderSubElement WHERE OrderId = {orderId} OrderElementId = {orderElementId}");
                return (List<SubOrderElement>)output;
            }
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}