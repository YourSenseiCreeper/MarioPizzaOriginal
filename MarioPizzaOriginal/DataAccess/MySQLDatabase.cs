using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;

namespace MarioPizzaOriginal.DataAccess
{
    public class MySQLDatabase : IMarioPizzaRepository 
    {
        public void AddElementToOrder(int orderId, Food element, double quantity)
        {
            throw new NotImplementedException();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public void AddOrder(MarioPizzaOrder order)
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
            throw new NotImplementedException();
        }

        public List<MarioPizzaOrder> GetAllOrders()
        {
            DbConnect db = new DbConnect();
            List<string>[] results = db.Select();
            Console.WriteLine("Database Results:");
            foreach (var element in results)
            {
                Console.WriteLine(element.ToString());
            }
            Console.WriteLine();
            return new List<MarioPizzaOrder>();
        }

        public List<Food> GetFilteredFood()
        {
            throw new NotImplementedException();
        }

        public Food GetFood(int foodId)
        {
            var sql = "SELECT F.FOOD_ID FROM FOOD";
            try
            {
                using (var connection = new SqlConnection(""))
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

        public Ingredient GetIngredient(int ingredientId)
        {
            throw new NotImplementedException();
        }

        public List<Ingredient> GetAllIngredients()
        {
            throw new NotImplementedException();
        }

        public List<Ingredient> GetIngredientsForFood(int foodId)
        {
            throw new NotImplementedException();
        }

        public MarioPizzaOrder GetOrder(int orderId)
        {
            var sql = "SELECT TOP 1 * FROM mariopizza.ORDERS";
            try
            {
                using (var connection = new SqlConnection(""))
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

        public Dictionary<Food, double> GetOrderElements(int orderId)
        {
            throw new NotImplementedException();
        }

        public OrderStatus GetOrderStatus(int orderId)
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
