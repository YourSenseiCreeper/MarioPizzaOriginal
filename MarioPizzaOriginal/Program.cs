using MarioPizzaOriginal.Controller;
using MarioPizzaOriginal.DataAccess;
using System;

namespace MarioPizzaOriginal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IMarioPizzaRepository dataAccess = new SqlLiteDatabase();
            OrderController orderC = new OrderController(dataAccess);
            IngredientController ingredientC = new IngredientController(dataAccess);
            FoodSizeSauceController foodC = new FoodSizeSauceController(dataAccess);
            //orderC.GetAllOrders();
            //orderC.GetOrder();
            //ingredientC.AllIngredients();
            //foodC.GetAllFood();
            foodC.GetFood();
            Console.WriteLine("Hello World!");
        }
    }
}
