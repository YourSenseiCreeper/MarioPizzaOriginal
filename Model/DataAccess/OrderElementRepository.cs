using Dapper;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Model.DataAccess
{
    internal class OrderElementRepository : BaseRepository<OrderElement>, IOrderElementRepository
    {
        private readonly OrmLiteConnectionFactory db;
        public OrderElementRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
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
            return db.Open().Select<OrderElement>($"SELECT * FROM OrderElement Where OrderId = {orderId}");
        }

        public int OrderElementNextId()
        {
            return db.Open().Scalar<OrderElement, int>(x => Sql.Max(x.OrderId)) + 1;
        }

        public void RemoveFromOrder(int orderId, int foodId)
        {
            db.Open().Delete<OrderElement>(x => x.OrderId == orderId && x.FoodId == foodId);
        }
    }
}