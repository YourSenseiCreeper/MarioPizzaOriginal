using Dapper;
using MarioPizzaOriginal.Domain;
using ServiceStack.OrmLite;
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
    public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public IngredientRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
        }
        
        public List<Ingredient> GetIngredientsForFood(int foodId)
        {
            return new List<Ingredient>();
            //return db.From<Ingredient>().Join<FoodIngredient>().Where();
            /*using (IDbConnection con = new SQLiteConnection(ConnectionString))
            {
                var query = "SELECT I.IngredientId, I.IngredientName, I.UnitOfMeasureType " +
                    "FROM Ingredients AS I " +
                    "JOIN FoodIngredient AS FI " +
                    "ON FI.IngredientId = I.IngredientId " +
                    $"WHERE FI.FoodId = {foodId}";
                return (List<Ingredient>)con.Query<Ingredient>(query);
            }
            */
        }
    }
}
