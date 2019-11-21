using MarioPizzaOriginal.Controller;
using MarioPizzaOriginal.DataAccess;
using System;

namespace MarioPizzaOriginal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IMarioPizzaRepository mpr = new AzureDatabase();
            OrderController orderC = new OrderController(mpr);
            orderC.GetOrder();
            Console.WriteLine("Hello World!");
        }
    }
}
