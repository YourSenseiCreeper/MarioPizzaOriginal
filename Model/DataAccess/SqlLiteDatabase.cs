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

        public void AddElementToOrder(int orderId, int foodId, double quantity)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Execute(
                    "INSERT INTO MarioPizzaOrderElement (OrderId,FoodId,Amount) " +
                    $"VALUES ({orderId},{foodId},{quantity})");
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
                var query = "INSERT INTO MarioPizzaOrder (ClientPhoneNumber,DeliveryAddress,Priority,Status,OrderTime) " +
                    $"VALUES ('{order.ClientPhoneNumber}','{order.DeliveryAddress}',{(int) order.Priority},{(int) order.Status},'{order.OrderTime.ToString("dd/MM/yyyy HH:MM:ss")}')";
                var output = con.Execute(query);
                //Adding OrderElements and SubOrderElements
                order.OrderElements.ForEach(orderElement =>
                {
                    AddElementToOrder(order.OrderId, orderElement.FoodId, orderElement.Amount);
                    orderElement.SubOrderElements.ForEach(subOrder => AddSubOrderElement(subOrder));
                });
            }
        }

        public void AddSubOrderElement(SubOrderElement subOrder)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "INSERT INTO MarioPizzaOrderSubElement (OrderElementId,FoodId,Amount) " +
                    $"VALUES ({subOrder.OrderElementId},{subOrder.FoodId},{subOrder.Amount})";
                var output = con.Execute(query);
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

        public List<MarioPizzaOrder> GetOrdersByStatus(OrderStatus status)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT " +
                    "M.OrderId,M.ClientPhoneNumber,M.DeliveryAddress," +
                    "M.Priority,M.Status,M.OrderTime " +
                    $"FROM MarioPizzaOrder AS M WHERE M.Status = {(int) status}";
                return (List<MarioPizzaOrder>)con.Query<MarioPizzaOrder>(query);
            }
        }

        public List<OrderElement> GetAllOrderElements()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Query<OrderElement>(
                    $"SELECT M.OrderElementId,M.FoodId,M.Amount FROM MarioPizzaOrderElement AS M");
                return (List<OrderElement>)output;
            }
        }

        public List<OrderElement> GetOrderElements(int orderId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Query<OrderElement>(
                    $"SELECT M.OrderElementId,M.FoodId,M.Amount FROM MarioPizzaOrderElement AS M WHERE M.OrderId = {orderId}");
                //SubOrderElements
                foreach(var row in output)
                {
                    row.SubOrderElements = GetSubOrderElements(row.OrderId, row.OrderElementId);
                }
                return (List<OrderElement>) output;
            }
        }

        public List<SubOrderElement> GetSubOrderElements(int orderId, int orderElementId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.Query<SubOrderElement>(
                    $"SELECT M.OrderElementId,M.FoodId,M.Amount " +
                    $"FROM MarioPizzaOrderSubElement AS M WHERE M.OrderElementId = {orderElementId}");
                return (List<SubOrderElement>) output;
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

        /// <summary>
        /// Returns bare order info without list of OrderElements
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public MarioPizzaOrder GetOrder(int orderId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.QueryFirst<MarioPizzaOrder>(
                    "SELECT " +
                    "M.OrderId,M.ClientPhoneNumber,M.DeliveryAddress," +
                    "M.Priority,M.Status,M.OrderTime " +
                    $"FROM MarioPizzaOrder AS M WHERE M.OrderId = {orderId}");
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
                var query = $"SELECT COUNT(*) FROM MarioPizzaOrder WHERE OrderId = {orderId}";
                bool exists = con.ExecuteScalar<int>(query) > 0;
                return exists;
            }
        }

        public bool OrderElementExists(int orderElementId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = $"SELECT COUNT(*) FROM MarioPizzaOrderElement WHERE OrderElementId = {orderElementId}";
                bool exists = con.ExecuteScalar<int>(query) > 0;
                return exists;
            }
        }

        public int OrderCount()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT COUNT(*) MarioPizzaOrder";
                return con.ExecuteScalar<int>(query);
            }
        }

        public int OrderElementsCount()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = "SELECT COUNT(*) MarioPizzaOrderElement";
                return con.ExecuteScalar<int>(query);
            }
        }

        public string GetFoodNameById(int foodId)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var output = con.QueryFirst<string>($"SELECT F.FoodName FROM Food AS F WHERE F.FoodId = {foodId}");
                return output;
            }
        }

        public void SaveData()
        {
            throw new NotImplementedException();
        }
    }
}
