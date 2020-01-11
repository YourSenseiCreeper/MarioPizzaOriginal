using Dapper;
using MarioPizzaOriginal.Domain;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Filter;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Tuple<string, double>> GetIngredients(int foodId)
        {
            string query = "SELECT I.IngredientName, FI.IngredientAmount FROM Ingredient AS I " +
                           "JOIN FoodIngredient AS FI ON FI.IngredientId = I.IngredientId " +
                           "LEFT JOIN Food AS F ON F.FoodId = FI.FoodId " +
                           $"WHERE F.FoodId = {foodId}";
            List<Tuple<string, double>> results = db.Open().Query<Tuple<string, double>>(query).ToList();
            foreach (var result in results)
            {
                //returnValues.Add(result.Item3, result.Item2.IngredientAmount);
                Console.WriteLine($"{result.Item1} - {result.Item2} xd");
            }
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
