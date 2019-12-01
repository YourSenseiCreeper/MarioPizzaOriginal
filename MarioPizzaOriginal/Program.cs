using MarioPizzaOriginal.Controller;
using MarioPizzaOriginal.DataAccess;
using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal
{
    public class Program
    {
        private static OrderController orderC;
        private static IngredientController ingredientC;
        private static FoodSizeSauceController foodC;
        private static OrderElementController orderElemC;

        public static void Main(string[] args)
        {
            IMarioPizzaRepository dataAccess = new SqlLiteDatabase();
            orderC = new OrderController(dataAccess);
            ingredientC = new IngredientController(dataAccess);
            foodC = new FoodSizeSauceController(dataAccess);
            orderElemC = new OrderElementController(dataAccess);

            bool exit = false;
            Console.WriteLine("Dostępne opcje:");
            List<string> options = new List<string> {
                "1. Składniki",
                "2. Zamówienia",
                "3. Produkty",
                "4. Wyjdź"};
            while (!exit)
            {
                options.ForEach(line => Console.WriteLine(line));
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1": Ingredients(); break;
                    case "2": Orders(); break;
                    case "3": Food(); break;
                    case "4": exit = true; break;
                    default:
                        Console.WriteLine($"Nie ma opcji: {input}!");
                        break;
                }
            }
            //orderC.GetAllOrders();
            //orderC.GetOrder();
            //ingredientC.AllIngredients();
            foodC.GetAllFood();
            //foodC.GetFood();
            Console.WriteLine("Hello World!");
        }

        private static void Ingredients()
        {
            Console.WriteLine("Dostępne opcje - Składniki:");
            List<string> options = new List<string> {
                "1. Wszystkie dostępne składniki",
                "2. Dodaj składnik",
                "3. Edytuj składnik",
                "4. Usuń składnik",
                "5. Powrót"};
            bool exit = false;
            while (!exit)
            {
                options.ForEach(line => Console.WriteLine(line));
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1": ingredientC.GetAllIngredients(); break;
                    case "2": ingredientC.AddIngredient(); break;
                    case "3": ingredientC.EditIngredient(); break;
                    case "4": ingredientC.DeleteIngredient(); break;
                    case "5": exit = true; break;
                    default:
                        Console.WriteLine($"Nie ma opcji: {input}!");
                        break;
                }
            }
        }

        private static void Orders()
        {
            Console.WriteLine("Dostępne opcje - Zamówienia:");
            List<string> options = new List<string> {
                "1. Lista wszystkich zamówień",
                "2. Oczekujące",
                "3. W trakcie",
                "4. Gotowe do dostarczenia",
                "5. Dodaj zamówienie",
                "6. Zmień zawartość zamówienia",
                "7. Usuń zamówienie",
                "8. Zmień status zamówienia",
                "9. Przenieś zamówienia do kolejnego etapu",
                "10. Zmień priorytet zamówienia",
                "11. Powrót"};
            options.ForEach(line => Console.WriteLine(line));
            var input = Console.ReadLine();
            bool exit = false;
            while (!exit)
            {
                switch (input)
                {
                    case "1": orderC.GetAllOrders(); break;
                    case "2": orderC.GetWaiting(); break;
                    case "3": orderC.GetInProgress(); break;
                    case "4": orderC.GetReadyForDelivery(); break;
                    case "5": orderC.AddOrder(); break;
                    case "6": OrderElements(); break;
                    case "7": orderC.DeleteOrder() break;
                    case "8": orderC.ChangeOrderStatus(); break;
                    case "9": orderC.MoveToNextStatus(); break;
                    case "10": orderC.ChangeOrderPriority(); break;
                    case "11": exit = true; break;
                    default:
                        Console.WriteLine($"Nie ma opcji: {input}!");
                        break;
                }
            }
        }

        private static void Food()
        {
            Console.WriteLine("Dostępne opcje - Produkty:");
            List<string> options = new List<string> {
                "1. Lista wszystkich produktów",
                "2. Szczegóły produktu",
                "3. Powrót"};
            options.ForEach(line => Console.WriteLine(line));
            var input = Console.ReadLine();
            bool exit = false;
            while (!exit)
            {
                switch (input)
                {
                    case "1": foodC.GetAllFood(); break;
                    case "2": foodC.GetFood(); break;
                    case "3": exit = true; break;
                    default:
                        Console.WriteLine($"Nie ma opcji: {input}!");
                        break;
                }
            }
        }

        private static void OrderElements()
        {
            Console.WriteLine("Dostępne opcje - Elementy zamówienia:");
            List<string> options = new List<string> {
                "1. Lista elementów zamówienia",
                "2. Dodaj element do zamówienia",
                "3. Zmień ilość",
                "4. Usuń element",
                "5. Powrót" };
            options.ForEach(line => Console.WriteLine(line));
            var input = Console.ReadLine();
            bool exit = false;
            while (!exit)
            {
                switch (input)
                {
                    case "1": foodC.GetAllFood(); break;
                    case "2": foodC.GetFood(); break;
                    case "3": exit = true; break;
                    default:
                        Console.WriteLine($"Nie ma opcji: {input}!");
                        break;
                }
            }
        }
    }
}
