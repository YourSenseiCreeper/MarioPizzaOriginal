using ServiceStack.OrmLite;
using System;
using System.Linq.Expressions;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class FoodRepository : BaseRepository<Food>, IFoodRepository
    {
        public FoodRepository()
        {
            using (var conn = connection.Open())
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
            var q = connection.Open().From<Food>().Join<Food, FoodIngredient>().Join<FoodIngredient, Ingredient>().Where(x => x.FoodId == foodId).Select("*");
            var results = connection.Open().SelectMulti<Food, FoodIngredient, Ingredient>(q);
            foreach(var result in results)
            {
                Console.WriteLine($"{result.Item3.IngredientName} - {result.Item2.IngredientAmount} {result.Item3.UnitOfMeasureType}");
            }
            Console.ReadLine();
            return 0;
        }

        public Food GetFoodWithIngredients(int id)
        {
            using (var dbConn = connection.Open())
            {
                var food = dbConn.SingleById<Food>(id);
                dbConn.LoadReferences(food);
                foreach (var foodIngredient in food.Ingredients)
                {
                    dbConn.LoadReferences(foodIngredient);
                }
                return food;
            }
        }
    }
}
