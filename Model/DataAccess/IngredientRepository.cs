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
    public class IngredientRepository : IIngredientRepository
    {
        public string ConnectionString => ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString;

        public void Add(Ingredient ingredient)
        {
            using (System.Data.IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                string insert = "INSERT INTO Ingredients (IngredientName,UnitOfMeasureType";
                string values = $" VALUES ('{ingredient.IngredientName}',{(int)ingredient.UnitOfMeasureType}";
                if (ingredient.PriceSmall != null)
                {
                    insert += ",PriceSmall";
                    values += $",'{ingredient.PriceSmall.ToString().Replace(",", ".")}'";
                }
                if (ingredient.PriceMedium != null)
                {
                    insert += ",PriceMedium";
                    values += $",'{ingredient.PriceMedium.ToString().Replace(",", ".")}'";
                }
                if (ingredient.PriceLarge != null)
                {
                    insert += ",PriceLarge";
                    values += $",'{ingredient.PriceLarge.ToString().Replace(",", ".")}'";
                }

                insert += ")";
                values += ")";
                con.Execute(insert + values);
            }
        }

        public int Count()
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"SELECT COUNT(*) Ingredients";
                return con.ExecuteScalar<int>(query);
            }
        }

        public void Edit(Ingredient editedIngredient)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                List<string> updateQuery = new List<string> {
                    "UPDATE Ingredients ",
                    editedIngredient.IngredientName != null ? $"SET IngredientName = {editedIngredient.IngredientName}" : "",
                    editedIngredient.PriceSmall != null ? $"SET PriceSmall = {editedIngredient.PriceSmall}" : "",
                    editedIngredient.PriceMedium != null ? $"SET PriceMedium = {editedIngredient.PriceMedium}" : "",
                    editedIngredient.PriceLarge != null ? $"SET PriceLarge = {editedIngredient.PriceLarge}" : "",
                    $"WHERE IngredientId = {editedIngredient.IngredientId}" };
                con.Execute(String.Join("", updateQuery));
            }
        }

        public bool Exists(int ingredientId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"SELECT COUNT(*) Ingredients WHERE IngredientId = {ingredientId}";
                return con.ExecuteScalar<int>(query) > 0;
            }
        }

        public Ingredient Get(int ingredientId)
        {
            /*
            using (var db = new MarioDBContext())
            {
                return db.Ingredients.Where(ing => ing.IngredientId == ingredientId).FirstOrDefault<Ingredient>();
            }
            */
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT I.IngredientId, I.IngredientName, I.UnitOfMeasureType, " +
                    "I.PriceSmall,I.PriceMedium,I.PriceLarge " +
                    "FROM Ingredients AS I " +
                    $"WHERE I.IngredientId = {ingredientId}";
                return con.QueryFirst<Ingredient>(query);
            }
        }

        public List<Ingredient> GetAll()
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT I.IngredientId,I.IngredientName,I.UnitOfMeasureType," +
                    "I.PriceSmall,I.PriceMedium,I.PriceLarge FROM Ingredients AS I ";
                return (List<Ingredient>)con.Query<Ingredient>(query);
            }
        }

        public List<Ingredient> GetIngredientsForFood(int foodId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT I.IngredientId, I.IngredientName, I.UnitOfMeasureType " +
                    "FROM Ingredients AS I " +
                    "JOIN FoodIngredient AS FI " +
                    "ON FI.IngredientId = I.IngredientId " +
                    $"WHERE FI.FoodId = {foodId}";
                return (List<Ingredient>)con.Query<Ingredient>(query);
            }
        }

        public void Remove(int ingredientId)
        {
            using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = $"DELETE FROM Ingredients AS I WHERE I.IngredientId = {ingredientId}";
                con.Execute(query);
            }
        }
    }
}
