using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using Model.Filter;
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
            return db.Open().Single<double>(
                "SELECT " +
                "SUM((F.Price * O.Amount) + " +
                "IFNULL((SELECT SUM((F2.Price * OE.Amount)) FROM OrderSubElement AS OE " +
                "INNER JOIN OrderElement O2 ON O2.OrderElementId = OE.OrderElementId " +
                "LEFT JOIN Food F2 ON F2.FoodId = OE.FoodId " +
                "WHERE O2.OrderElementId = O.OrderElementId), 0)" +
                ") AS Cena " +
                "FROM Food F " +
                "JOIN OrderElement O ON O.FoodId = F.FoodId " +
                $"WHERE O.OrderId = {orderId}");
        }

        public List<MarioPizzaOrder> Filter(OrderFilter filter)
        {
            /*=
            int and = 0;
            string foodNameSql = !string.IsNullOrEmpty(filter.FoodName) ? ((and++ > 0 ? " AND " : "") + $" FoodName LIKE '%{filter.FoodName}%' ") : "";
            string query = filter == null ? "SELECT * FROM Food " : "SELECT * FROM Food WHERE " +
                sqlStringForRange(ref and, "FoodId", (double?)filter.FoodIdMin, (double?)filter.FoodIdMax) +
                foodNameSql +
                sqlStringForRange(ref and, "NettPrice", filter.NettPriceMin, filter.NettPriceMax) +
                sqlStringForRange(ref and, "Price", filter.PriceMin, filter.PriceMax) +
                sqlStringForRange(ref and, "Weight", filter.WeightMin, filter.WeightMax) +
                sqlStringForRange(ref and, "ProductionTime", (double?)filter.ProductionTimeMin, (double?)filter.ProductionTimeMax);
            //sqlStringForRange(and, filter.PriceSmallMin, filter.PriceSmallMax) +
            //sqlStringForRange(and, filter.PriceMediumMin, filter.PriceMediumMax) +
            //sqlStringForRange(and, filter.PriceLargeMin, filter.PriceLargeMax);
            */
            return db.Open().Select<MarioPizzaOrder>("SELECT * FROM MarioPizzaOrder");
        }

        private string sqlStringForRange(ref int and, string key, double? minValue, double? maxValue)
        {
            string result = "";
            if (minValue != null)
            {
                if (and++ > 0) result += " AND ";
                result += $"{key} >= {minValue} ";
            }
            if (maxValue != null)
            {
                if (and++ > 0) result += " AND ";
                result += $"{key} <= {maxValue} ";
            }
            return result;
        }
    }
}