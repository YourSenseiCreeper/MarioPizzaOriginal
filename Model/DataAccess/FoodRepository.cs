﻿using Dapper;
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
    public class FoodRepository : BaseRepository<Food>, IFoodRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public FoodRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
        }
        /*
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
        */
        public string GetName(int foodId)
        {
            return db.Open().Single<Food>(x => x.FoodId == foodId).FoodName;
        }

        public double CalculatePriceForFood(int foodId)
        {
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
    }
}
