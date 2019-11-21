using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MarioPizzaOriginal.Model;
using MarioPizzaOriginal.Model.Enums;

namespace MarioPizzaOriginal.DataAccess
{
    public class AzureDatabase : IMarioPizzaRepository
    {
        private readonly string azurePass = "3e79670283200091e81e365cd48edbee!@#";
        private string ConnStr()
        {
            return $"Server=tcp:mariopizza.database.windows.net,1433;Initial Catalog=mariopizza;Persist Security Info=False;" +
                $"User ID=gastherr;Password={azurePass};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;" +
                $"Connection Timeout=30;";
        }
        
        public void AddDrink(Drink drink)
        {
            throw new NotImplementedException();
        }

        public void AddElementToOrder(int orderId, FoodSizeSauce element, double quantity)
        {
            throw new NotImplementedException();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public void AddKebab(Kebab kebab)
        {
            throw new NotImplementedException();
        }

        public void AddOrder(MarioPizzaOrder order)
        {
            throw new NotImplementedException();
        }

        public void AddPizza(Pizza pizza)
        {
            throw new NotImplementedException();
        }

        public void AddTortilla(Tortilla tortilla)
        {
            throw new NotImplementedException();
        }

        public void ChangeOrderPriority(int orderId, OrderPriority newOrderPriority)
        {
            throw new NotImplementedException();
        }

        public void ChangeOrderStatus(int orderId, OrderStatus newOrderStatus)
        {
            throw new NotImplementedException();
        }

        public void DeleteDrink(int foodId)
        {
            throw new NotImplementedException();
        }

        public void DeleteElementFromOrder(int orderId, string elementName)
        {
            throw new NotImplementedException();
        }

        public void DeleteElementFromOrder(int orderId, int foodId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteIngredient(string ingredientName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteIngredient(int ingredientId)
        {
            throw new NotImplementedException();
        }

        public void DeleteKebab(int foodId)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public void DeletePizza(int foodId)
        {
            throw new NotImplementedException();
        }

        public void DeleteTortilla(int foodId)
        {
            throw new NotImplementedException();
        }

        public void EditDrink(Drink editedDrink)
        {
            throw new NotImplementedException();
        }

        public void EditIngredient(Ingredient editedIngredient)
        {
            throw new NotImplementedException();
        }

        public void EditKebab(Kebab editedKebab)
        {
            throw new NotImplementedException();
        }

        public void EditPizza(Pizza editedPizza)
        {
            throw new NotImplementedException();
        }

        public void EditTortilla(Tortilla editedTortilla)
        {
            throw new NotImplementedException();
        }

        public List<FoodSizeSauce> GetAllFood()
        {
            throw new NotImplementedException();
        }

        public List<MarioPizzaOrder> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Drink GetDrink(int foodId)
        {
            throw new NotImplementedException();
        }

        public List<FoodSizeSauce> GetFilteredFood()
        {
            throw new NotImplementedException();
        }

        public FoodSizeSauce GetFood(int foodId)
        {
            var sql = "SELECT " +
                "F.FOOD_ID FROM FOOD";
            try
            {
                using (var connection = new SqlConnection(ConnStr()))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} , {1} , {2} , {3} , {4}",
                                    reader.GetGuid(0),
                                    reader.GetString(1),
                                    reader.GetInt32(2),
                                    (reader.IsDBNull(3)) ? "NULL" : reader.GetString(3),
                                    (reader.IsDBNull(4)) ? "NULL" : reader.GetString(4));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            throw new NotImplementedException();
        }

        public Ingredient GetIngredient(string ingredientName)
        {
            throw new NotImplementedException();
        }

        public Ingredient GetIngredient(int ingredientId)
        {
            throw new NotImplementedException();
        }

        public List<Ingredient> GetIngredientList()
        {
            throw new NotImplementedException();
        }

        public Kebab GetKebab(int foodId)
        {
            throw new NotImplementedException();
        }
        public MarioPizzaOrder GetOrder(int orderId)
        {
            var sql = "SELECT TOP 1 * FROM mariopizza.ORDERS";
            try
            {
                using (var connection = new SqlConnection(ConnStr()))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} , {1} , {2} , {3} , {4} , {5} , {6}",
                                    reader.GetInt32(0),
                                    reader.GetInt32(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetInt16(4),
                                    reader.GetInt16(5),
                                    reader.GetDateTime(6));
                            }
                            Console.WriteLine($"Są wyniki: {reader.HasRows}");
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            return new MarioPizzaOrder();
        }

        public OrderStatus GetOrderStatus(int orderId)
        {
            throw new NotImplementedException();
        }

        public Pizza GetPizza(int foodId)
        {
            throw new NotImplementedException();
        }

        public Tortilla GetTortilla(int foodId)
        {
            throw new NotImplementedException();
        }

        public bool OrderExists(int orderId)
        {
            throw new NotImplementedException();
        }

        public void SaveData()
        {
            throw new NotImplementedException();
        }
    }
}
