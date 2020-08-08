using MarioPizzaOriginal.Domain;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
namespace Model.DataAccess
{
    public class FoodRepository : BaseRepository<Food>, IFoodRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public FoodRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
            using (var conn = dbConnection.Open())
            {
                if (!conn.TableExists<Food>())
                {
                    conn.CreateTable<Food>();
                    conn.Save(new Food
                    {
                        FoodId = 1,
                        FoodName = "TestFood",
                        Price = 2,
                        NettPrice = 1,
                        ProductionTime = 90,
                        Weight = 0.5
                    });
                }
            }
        }

        public string GetName(int foodId) => Get(foodId).FoodName;

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
            string query = $@"SELECT I.IngredientName, FI.IngredientAmount FROM Ingredient AS I
                           JOIN FoodIngredient AS FI ON FI.IngredientId = I.IngredientId
                           LEFT JOIN Food AS F ON F.FoodId = FI.FoodId
                           WHERE F.FoodId = {foodId}";
            return db.Open().Dictionary<string, double>(query) ?? new Dictionary<string, double>();
        }
    }
}
