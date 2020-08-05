using MarioPizzaOriginal.Account;
using MarioPizzaOriginal.Controller;
using MarioPizzaOriginal.Domain;
using System;
using System.Collections.Generic;
using TinyIoC;

namespace MarioPizzaOriginal
{
    public class Program
    {
        private static OrderController orderC;
        private static IngredientController ingredientC;
        private static FoodController foodC;
        private static OrderElementController orderElemC;
        private static User user;

        public static void Main(string[] args)
        {
            //IMarioPizzaRepository repository = new MarioPizzaRepository();
            //IList<string> lisa = new List<string>();
            var container = TinyIoCContainer.Current;
            new DomainStartup(container); // registering repositories to IoC
            orderC = new OrderController(container);
            ingredientC = new IngredientController(container);
            foodC = new FoodController(container);
            orderElemC = new OrderElementController(container);
            
            user = GetUser();

            Menu("Dostępne opcje: ",
                new Dictionary<string, Action>
                {
                    { "Składniki",          new Action(Ingredients) },
                    { "Zamówienia",         new Action(Orders) },
                    { "Elementy zamówienia", new Action(OrderElements) },
                    { "Produkty",           new Action(Food) }
                },
                "Wyjdź");
        }

        private static void Ingredients()
        {
            Menu("Dostępne opcje - składniki: ",
                new Dictionary<string, Action>
                {
                    { "Wszystkie dostępne składniki",   new Action(ingredientC.GetAllIngredients) },
                    { "Szczegóły składnika",            new Action(ingredientC.GetIngredient) },
                    { "Dodaj składnik",                 new Action(ingredientC.AddIngredient) },
                    { "Edytuj składnik",                new Action(ingredientC.EditIngredient) },
                    { "Usuń składnik",                  new Action(ingredientC.DeleteIngredient) }
                },
                "Powrót");
        }

        private static void Orders()
        {
            Menu("Dostępne opcje - Zamówienia: ",
                new Dictionary<string, Action>
                {
                    { "Lista wszystkich zamówień",              new Action(orderC.GetAllOrders) },
                    { "Zawartość zamówienia",                   new Action(orderC.GetOrder) },
                    { "Oczekujące",                             new Action(orderC.GetOrdersWaiting) },
                    { "W trakcie",                              new Action(orderC.GetOrdersInProgress) },
                    { "Gotowe do dostarczenia",                 new Action(orderC.GetOrdersReadyForDelivery) },
                    { "Dodaj zamówienie",                       new Action(orderC.AddOrder) },
                    //{ "Zmień zawartość zamówienia",             new Action(orderC.EditOrder) },
                    { "Usuń zamówienie",                        new Action(orderC.DeleteOrder) },
                    { "Zmień status zamówienia",                new Action(orderC.ChangeOrderStatus) },
                    { "Przenieś zamówienia do kolejnego etapu", new Action(orderC.MoveToNextStatus) },
                    { "Zmień priorytet zamówienia",             new Action(orderC.ChangeOrderPriority) },
                    { "Policz cenę dla zamówienia",             new Action(orderC.CalculatePriceForOrder) },
                    { "Wszystkie podelementy zamówienia",       new Action(orderC.ShowAllSubOrderElements) }
                },
                "Powrót");
        }

        private static void Food()
        {
            Menu("Dostępne opcje - Produkty:",
                new Dictionary<string, Action>
                {
                    { "Lista wszystkich produktów", new Action(foodC.GetAllFood) },
                    { "Szczegóły produktu", new Action(foodC.GetFood) },
                    { "Dodaj produkt", new Action(foodC.AddFood) },
                    { "Usuń produkt", new Action(foodC.DeleteFood) },
                    { "Szukaj wg filtru", new Action(foodC.GetFilteredFood) }
                }, "Powrót");
        }

        private static void OrderElements()
        {
            Menu("Dostępne opcje - Elementy zamówienia:",
                new Dictionary<string, Action>
                {
                    { "Lista wszystkich elementów zamówień", new Action(orderElemC.GetAllOrderElements) },
                    { "Lista elementów zamówienia", new Action(orderElemC.GetAllElementsForOrder) },
                    { "Dodaj element do zamówienia", new Action(orderElemC.AddOrderElement)},
                    { "Zmień ilość", new Action(orderElemC.ChangeAmount) },
                    { "Usuń element", new Action(orderElemC.DeleteOrderElement) }
                }, "Powrót");
        }

        private static void Menu(string header, Dictionary<string, Action> keyValues, string footer)
        {
            bool exit = false;
            int input;
            int index = 1;

            keyValues.Add(footer, null); // Last option - return
            List<string> keys = new List<string>();
            List<Action> values = new List<Action>();
            foreach (var entry in keyValues)
            {
                if (entry.Value == null || user.Permissions.Contains(entry.Value.Method.Name)){
                    keys.Add($"{index++}. {entry.Key}");
                    values.Add(entry.Value);
                }
            }
            do
            {
                Console.Clear();
                Console.WriteLine(header);
                keys.ForEach(x => Console.WriteLine(x));
                input = ViewHelper.AskForInt("", clear: false); //Waiting for answer
                if (input > 0 && input <= values.Count)
                {
                    if (input == values.Count) exit = true;
                    else values[input - 1].DynamicInvoke();
                }
                else ViewHelper.WriteAndWait($"Nie ma opcji: {input}!");
            } while (!exit);
        }

        private static User GetUser()
        {
            AccountType account = ViewHelper.AskForOption<AccountType>("Dostępne rodzaj konta", "Wpisz nazwę konta, na które chcesz się zalogować: ");
            if (account == AccountType.ROOT) return new Root();
            else if (account == AccountType.CASHIER) return new Cashier();
            else return new Driver();
        }
    }
}
