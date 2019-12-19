using Dapper;
using MarioPizzaOriginal.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccess
{
    public class FoodRepository : IFoodRepository
    {
        public string ConnectionString => ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString;

        public void Add(Food newOne)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public void Edit(Food editOne)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                List<string> updateQuery = new List<string> {
                    "UPDATE Food ",
                    editOne.FoodName != "" ? $"SET FoodName = {editOne.FoodName}" : "",
                    editOne.NettPrice != null ? $"SET NettPrice = {editOne.NettPrice}" : "",
                    editOne.Price != null ? $"SET Price = {editOne.Price}" : "",
                    editOne.Weight != null ? $"SET Weight = {editOne.Weight}" : "",
                    editOne.ProductionTime != null ? $"SET ProductionTime = {editOne.ProductionTime}" : "",
                    $"WHERE FoodId = {editOne.FoodId}" };
                con.Execute(String.Join("", updateQuery));
                if (editOne.Ingredients != null)
                {
                    // It doesnt' check if the ingredient was removed or added!
                    // Need to fix that
                    //editOne.Ingredients.ForEach(ing => EditIngredient(ing));
                }
            }
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Food Get(int foodId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var output = con.QueryFirst<Food>(
                    "SELECT FoodId,FoodName,NettPrice,Price,Weight,ProductionTime " +
                    $"FROM Food WHERE FoodId = {foodId}");
                return output;
            }

        }

        public List<Food> GetAll()
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT " +
                    "FoodId,FoodName,NettPrice," +
                    "Price,Weight,ProductionTime " +
                    "FROM Food";
                var output = con.Query<Food>(query);
                return (List<Food>)output;
            }
        }

        public string GetName(int foodId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"SELECT FoodName FROM Food WHERE FoodId = {foodId}";
                return con.QueryFirst<string>(query);
            }
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public double CalculatePriceForFood(int foodId)
        {
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
            return 0;
        }
    }
}
