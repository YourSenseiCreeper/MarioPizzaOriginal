using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Text;
using Dapper;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using Model;

namespace MarioPizzaOriginal.DataAccess
{
    public class SqlLiteDatabase : IMarioPizzaRepository
    {
        private static string LoadConnectionString(string id = "SqlLite")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public void AddElementToOrder(int orderId, Food element, double quantity)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Execute(
                    "INSERT INTO MarioPizzaOrderElement (OrderId,FoodId,Amount) " +
                    $"VALUES ({orderId},{element.FoodId},{quantity})");
            }
        }

        public void AddIngredient(Ingredient ingredient)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Execute(
                    "INSERT INTO Ingredients (IngredientName,UnitOfMeasureType,PriceSmall,PriceMedium,PriceLarge) " +
                    $"VALUES ('{ingredient.IngredientName}',{ingredient.PriceSmall},{ingredient.PriceMedium},{ingredient.PriceLarge})");
            }
        }

        public void AddOrder(MarioPizzaOrder order)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Execute(
                    "INSERT INTO MarioPizzaOrder (ClientPhoneNumber,DeliveryAddress,Priority,Status,OrderTime) " +
                    $"VALUES ('{order.ClientPhoneNumber}','{order.DeliveryAddress}',{order.Priority},{order.Status},'{order.OrderTime}')");
                foreach(var orderElement in order.OrderList)
                {
                    AddElementToOrder(order.OrderId, orderElement.Key, orderElement.Value);
                    foreach (var subOrderElement in orderElement)
                    {

                    }
                }
                
            }
        }

        public void AddSubOrderElement(MarioPizzaOrder order)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Execute(
                    "INSERT INTO Ingredients (IngredientName,UnitOfMeasureType,PriceSmall,PriceMedium,PriceLarge) " +
                    $"VALUES ('{ingredient.IngredientName}',{ingredient.PriceSmall},{ingredient.PriceMedium},{ingredient.PriceLarge})");
            }
        }

        public void ChangeOrderPriority(int orderId, OrderPriority newOrderPriority)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Execute(
                    "UPDATE MarioPizzaOrder M SET" +
                    $"M.Priority = {(int)newOrderPriority} WHERE M.OrderId = {orderId}");
            }
        }

        public void ChangeOrderStatus(int orderId, OrderStatus newOrderStatus)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = $"UPDATE MarioPizzaOrder SET Status = {(int)newOrderStatus} WHERE OrderId = {orderId}";
                var output = con.Execute(query);
            }
        }

        public void DeleteElementFromOrder(int orderId, int foodId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteIngredient(int ingredientId)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public void EditIngredient(Ingredient editedIngredient)
        {
            throw new NotImplementedException();
        }

        public List<Food> GetAllFood()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT " +
                    "F.FoodId,F.FoodName,F.NettPrice," +
                    "F.Price,F.Weight,F.ProductionTime " +
                    "FROM Food F";
                var output = con.Query<Food>(query);
                foreach (var row in output)
                {
                    row.Ingredients = GetIngredientsForFood(row.FoodId);
                }
                return (List<Food>) output;
            }
        }

        public List<MarioPizzaOrder> GetAllOrders()
        {
            using(IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT " +
                    "M.OrderId,M.ClientPhoneNumber,M.DeliveryAddress," +
                    "M.Priority,M.Status,M.OrderTime " +
                    "FROM MarioPizzaOrder AS M";
                return (List<MarioPizzaOrder>) con.Query<MarioPizzaOrder>(query);
            }
        }

        public List<MarioPizzaOrder> GetOrdersWaiting()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT " +
                    "M.OrderId,M.ClientPhoneNumber,M.DeliveryAddress," +
                    "M.Priority,M.Status,M.OrderTime " +
                    "FROM MarioPizzaOrder AS M WHERE M.Status = 0";
                return (List<MarioPizzaOrder>)con.Query<MarioPizzaOrder>(query);
            }
        }

        public List<MarioPizzaOrder> GetOrdersInProgress()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT " +
                    "M.OrderId,M.ClientPhoneNumber,M.DeliveryAddress," +
                    "M.Priority,M.Status,M.OrderTime " +
                    "FROM MarioPizzaOrder AS M WHERE M.Status = 1";
                return (List<MarioPizzaOrder>)con.Query<MarioPizzaOrder>(query);
            }
        }

        public List<MarioPizzaOrder> GetOrdersReadyForDelivery()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT " +
                    "M.OrderId,M.ClientPhoneNumber,M.DeliveryAddress," +
                    "M.Priority,M.Status,M.OrderTime " +
                    "FROM MarioPizzaOrder AS M WHERE M.Status = 2";
                return (List<MarioPizzaOrder>)con.Query<MarioPizzaOrder>(query);
            }
        }

        public Dictionary<Food, double> GetOrderElements(int orderId)
        {
            var result = new Dictionary<Food, double>();
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Query<(int, double)>(
                    $"SELECT M.FoodId,M.Amount FROM MarioPizzaOrderElement AS M WHERE M.OrderId = {orderId}");
                foreach(var row in output)
                {
                    var food = GetFood(row.Item1);
                    result.Add(food, row.Item2);
                }
                return result;
            }
        }

        public List<Food> GetFilteredFood()
        {
            return new List<Food>();
        }

        public Food GetFood(int foodId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.QueryFirst<Food>(
                    "SELECT F.FoodId,F.FoodName,F.NettPrice,F.Price," +
                    "F.Weight,F.ProductionTime " +
                    $"FROM Food AS F WHERE F.FoodId = {foodId}");
                output.Ingredients = GetIngredientsForFood(output.FoodId);
                return output;
            }
        }

        public List<Ingredient> GetIngredientsForFood(int foodId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT I.IngredientId, I.IngredientName, I.UnitOfMeasureType, I.AmountOfUOM " +
                    "FROM Ingredients AS I " +
                    "JOIN FoodIngredient AS FI " +
                    "ON I.IngredientId = FI.IngredientId " +
                    $"WHERE FI.FoodId = {foodId}";
                return (List<Ingredient>) con.Query<Ingredient>(query);
            }
        }

        public Ingredient GetIngredient(int ingredientId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT I.IngredientId, I.IngredientName, I.UnitOfMeasureType, I.AmountOfUOM " +
                    "FROM Ingredients AS I " +
                    $"WHERE I.IngredientId = {ingredientId}";
                return con.QueryFirst<Ingredient>(query);
            }
        }

        public List<Ingredient> GetAllIngredients()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT I.IngredientId, I.IngredientName, I.UnitOfMeasureType, I.AmountOfUOM FROM Ingredients AS I ";
                return (List<Ingredient>) con.Query<Ingredient>(query);
            }
        }

        public MarioPizzaOrder GetOrder(int orderId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.QueryFirst<MarioPizzaOrder>(
                    "SELECT " +
                    "M.OrderId,M.ClientPhoneNumber,M.DeliveryAddress," +
                    "M.Priority,M.Status,M.OrderTime " +
                    "FROM MarioPizzaOrder AS M", new DynamicParameters());
                output.OrderList = GetOrderElements(orderId);
                return output;
            }
        }

        public OrderStatus GetOrderStatus(int orderId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = $"SELECT O.Status MarioPizzaOrder AS O WHERE O.OrderId = {orderId}";
                return con.QueryFirst<OrderStatus>(query);
            }
        }

        public bool OrderExists(int orderId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = $"SELECT COUNT(*) MarioPizzaOrder AS O WHERE O.OrderId = {orderId}";
                bool exists = con.ExecuteScalar<int>(query) > 0;
                return exists;
            }
        }

        public void SaveData()
        {
            throw new NotImplementedException();
        }
    }
}
