﻿using MarioPizzaOriginal.Domain;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using MarioPizzaOriginal;

namespace Model.DataAccess
{
    public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public IngredientRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
            using (var conn = dbConnection.Open())
            {
                if (conn.CreateTableIfNotExists<Ingredient>())
                {
                    conn.Insert(
                        new Ingredient
                        {
                            IngredientId = 1,
                            IngredientName = "TestIngredient",
                            PriceSmall = 1,
                            PriceMedium = 2,
                            PriceLarge = 3,
                            UnitOfMeasureType = UnitOfMeasure.KILOGRAM
                        });
                }
            }
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
