using MarioPizzaOriginal.Domain;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Filter;
using System.Linq;

namespace Model.DataAccess
{
    public class FoodRepository : BaseRepository<Food>, IFoodRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public FoodRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
        }

        public string GetName(int foodId)
        {
            return db.Open().Single<Food>(x => x.FoodId == foodId).FoodName;
        }

        public double CalculatePriceForFood(int foodId)
        {
            var q = db.Open().From<Food>().Join<Food, FoodIngredient>().Join<FoodIngredient, Ingredient>().Where(x => x.FoodId == foodId).Select("*");
            var results = db.Open().SelectMulti<Food, FoodIngredient, Ingredient>(q);
            foreach(var result in results)
            {
                Console.WriteLine($"{result.Item3.IngredientName} - {result.Item2.IngredientAmount} {result.Item3.UnitOfMeasureType}");
            }
            Console.ReadLine();
            /*
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"SELECT F.FoodId,F.Price,I.IngredientId,FI.IngredientAmount,I.PriceSmall,I.PriceMedium,I.PriceLarge " +
                            "FROM Food AS F " +
                            "JOIN FoodIngredient AS FI ON FI.FoodId = F.FoodId " +
                            "JOIN Ingredients AS I ON I.IngredientId = FI.IngredientId" +
                            $"WHERE F.FoodId = {foodId}";
                //Define type and return values for the query
                var output = con.Query(query);
            }
            */
            return 0;
        }

        public Dictionary<string, double> GetIngredients(int foodId)
        {
            string query = "SELECT I.IngredientName, FI.IngredientAmount FROM Ingredient AS I " +
                           "JOIN FoodIngredient AS FI ON FI.IngredientId = I.IngredientId " +
                           "LEFT JOIN Food AS F ON F.FoodId = FI.FoodId " +
                           $"WHERE F.FoodId = {foodId}";
            var results = db.Open().Dictionary<string, double>(query);
            if (results == null) return new Dictionary<string, double>();
            return results;
        }

        public List<Food> Filter(FoodFilter filter)
        {
            int and = 0;
            string foodNameSql = !string.IsNullOrEmpty(filter.FoodName) ? ((and++ > 0 ? " AND " : "") + $" FoodName LIKE '%{filter.FoodName}%' ") : "";
            string query = filter == null ? "SELECT * FROM Food " : "SELECT * FROM Food WHERE " +
                sqlStringForRange(ref and, "FoodId", (double?) filter.FoodIdMin, (double?) filter.FoodIdMax) +
                foodNameSql +
                sqlStringForRange(ref and, "NettPrice", filter.NettPriceMin, filter.NettPriceMax) +
                sqlStringForRange(ref and, "Price", filter.PriceMin, filter.PriceMax) +
                sqlStringForRange(ref and, "Weight", filter.WeightMin, filter.WeightMax) +
                sqlStringForRange(ref and, "ProductionTime", (double?) filter.ProductionTimeMin, (double?) filter.ProductionTimeMax);
                //sqlStringForRange(and, filter.PriceSmallMin, filter.PriceSmallMax) +
                //sqlStringForRange(and, filter.PriceMediumMin, filter.PriceMediumMax) +
                //sqlStringForRange(and, filter.PriceLargeMin, filter.PriceLargeMax);
            return db.Open().Select<Food>(query);
        }

        private string sqlStringForRange(ref int and, string key, double? minValue, double? maxValue)
        {
            string result = "";
            if(minValue != null)
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
