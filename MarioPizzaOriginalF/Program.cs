using MarioPizzaOriginal.Controller;
using Model.DataAccess;
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
            IMarioPizzaRepository repository = new MarioPizzaRepository();
            orderC = new OrderController(repository.FoodRepository, repository.OrderRepository, repository.OrderElementRepository, repository.OrderSubElementRepository);
            ingredientC = new IngredientController(repository.IngredientRepository);
            foodC = new FoodSizeSauceController(repository.FoodRepository);
            orderElemC = new OrderElementController(repository.OrderElementRepository);

            List<string> options = new List<string> {
                "1. Składniki",
                "2. Zamówienia",
                "3. Elementy zamówienia",
                "4. Produkty",
                "5. Wyjdź"};
            bool exit = false;
            string input;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Dostępne opcje:");
                options.ForEach(line => Console.WriteLine(line));
                input = Console.ReadLine();
                switch (input)
                {
                    case "1": Ingredients(); break;
                    case "2": Orders(); break;
                    case "3": OrderElements(); break;
                    case "4": Food(); break;
                    case "5": exit = true; break;
                    default:
                        Console.WriteLine($"Nie ma opcji: {input}!");
                        break;
                }
            }
        }

        private static void Ingredients()
        {
            Console.WriteLine("Dostępne opcje - Składniki:");
            List<string> options = new List<string> {
                "1. Wszystkie dostępne składniki",
                "2. Szczegóły składnika",
                "3. Dodaj składnik",
                "4. Edytuj składnik",
                "5. Usuń składnik",
                "6. Powrót"};
            string input;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                options.ForEach(line => Console.WriteLine(line));
                input = Console.ReadLine();
                switch (input)
                {
                    case "1": ingredientC.GetAllIngredients(); break;
                    case "2": ingredientC.GetIngredient(); break;
                    case "3": ingredientC.AddIngredient(); break;
                    case "4": ingredientC.EditIngredient(); break;
                    case "5": ingredientC.DeleteIngredient(); break;
                    case "6": exit = true; break;
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
                "2. Zawartość zamówienia",
                "3. Oczekujące",
                "4. W trakcie",
                "5. Gotowe do dostarczenia",
                "6. Dodaj zamówienie",
                "7. Zmień zawartość zamówienia",
                "8. Usuń zamówienie",
                "9. Zmień status zamówienia",
                "10. Przenieś zamówienia do kolejnego etapu",
                "11. Zmień priorytet zamówienia",
                "12. Powrót"};
            string input;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                options.ForEach(line => Console.WriteLine(line));
                input = Console.ReadLine();
                switch (input)
                {
                    case "1": orderC.GetAllOrders(); break;
                    case "2": orderC.GetOrder(); break;
                    case "3": orderC.GetOrdersWaiting(); break;
                    case "4": orderC.GetOrdersInProgress(); break;
                    case "5": orderC.GetOrdersReadyForDelivery(); break;
                    case "6": orderC.AddOrder(); break;
                    case "7": OrderElements(); break;
                    case "8": orderC.DeleteOrder(); break;
                    case "9": orderC.ChangeOrderStatus(); break;
                    case "10": orderC.MoveToNextStatus(); break;
                    case "11": orderC.ChangeOrderPriority(); break;
                    case "12": exit = true; break;
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
            string input;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                options.ForEach(line => Console.WriteLine(line));
                input = Console.ReadLine();
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
                "1. Lista wszystkich elementów zamówień",
                "2. Lista elementów zamówienia",
                "3. Dodaj element do zamówienia",
                "4. Zmień ilość",
                "5. Usuń element",
                "6. Powrót" };
            string input;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                options.ForEach(line => Console.WriteLine(line));
                input = Console.ReadLine();
                switch (input)
                {
                    case "1": orderElemC.GetAllOrderElements(); break;
                    case "2": orderElemC.GetAllElementsForOrder(); break;
                    case "3": orderElemC.AddOrderElement(); break;
                    case "4": orderElemC.ChangeAmount(); break;
                    case "5": orderElemC.DeleteOrderElement(); break;
                    case "6": exit = true; break;
                    default:
                        Console.WriteLine($"Nie ma opcji: {input}!");
                        break;
                }
            }
        }
    }
}
