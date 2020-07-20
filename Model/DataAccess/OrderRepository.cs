using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using Model.Filter;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Model.DataAccess
{
    public class OrderRepository : BaseRepository<MarioPizzaOrder>, IOrderRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public OrderRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
            using (var conn = dbConnection.Open())
            {
                if (conn.CreateTableIfNotExists<MarioPizzaOrder>())
                {
                    conn.Insert(
                        new MarioPizzaOrder
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
            string result = "";
            int and = 0;
            var properties = filter.GetType().GetProperties();
            List<PropertyInfo> mins = new List<PropertyInfo>();
            List<PropertyInfo> maxes = new List<PropertyInfo>();
            List<PropertyInfo> names = new List<PropertyInfo>();

            var propertyData = new Dictionary<string, MinMaxPair>();
            /*
            string propertyName = "";
            foreach (var property in properties)
            {
                propertyName = property.Name.ToLower().Replace("min", "").Replace("max", "");
                if (!propertyData.ContainsKey(propertyName))
                {
                    propertyData.Add(propertyName, new MinMaxPair());
                }
                // what happens to date???
                var minmax = new MinMaxPair<Type> ();

                if (property.Name.ToLower().EndsWith("min")) propertyData[propertyName].Min = (double?) property.GetValue(filter);
                else if (property.Name.ToLower().EndsWith("max")) propertyData[propertyName].Max = (double?) property.GetValue(filter);
                // if property inherits from Enum or is String
                //else propertyData[propertyName] = new MinMaxPair();

                if (property.PropertyType == typeof(string)) 
                {
                    string value = (string) property.GetValue(filter);
                    result += !string.IsNullOrEmpty(value) ? ((and++ > 0 ? " AND " : "") + $" FoodName LIKE '%{value}%' ") : "";
                }
                else
                {
                    Type type = property.PropertyType == typeof(double?) ? typeof(double?) : typeof(int?);
                    var value = property.GetValue(filter); 
                }
            }
            int and = 0;
            string orderSql = !string.IsNullOrEmpty(filter.) ? ((and++ > 0 ? " AND " : "") + $" FoodName LIKE '%{filter.FoodName}%' ") : "";
            string query = filter == null ? "SELECT * FROM MarioPizzaOrder " : "SELECT * FROM MarioPizzaOrder WHERE " +
                sqlStringForRange(ref and, "ClientPhoneNumber", filter.OrderIdMin, filter.OrderIdMax) +
                sqlStringForRange(ref and, "OrderId", filter.OrderIdMin, filter.OrderIdMax) +
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