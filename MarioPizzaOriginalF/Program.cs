using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Account;
using MarioPizzaOriginal.Controller;
using MarioPizzaOriginal.Domain;
using TinyIoC;

namespace MarioPizzaOriginal
{
    public class Program
    {
        private static OrderController orderC;
        private static IngredientController ingredientC;
        private static FoodController foodC;
        private static OrderElementController orderElemC;
        private static UserController userC;
        private static BaseRights user;
        private static TinyIoCContainer container;
        public static void Main(string[] args)
        {
            //IMarioPizzaRepository repository = new MarioPizzaRepository();
            //IList<string> lisa = new List<string>();
            container = TinyIoCContainer.Current;
            new DomainStartup(container); // registering repositories to IoC
            container.Register(new User(), "CurrentUser");
            userC = new UserController(container);
            orderC = new OrderController(container);
            ingredientC = new IngredientController(container);
            foodC = new FoodController(container);
            orderElemC = new OrderElementController(container);
            
            user = new BaseRights();
            UserAuthentication();

            Menu("Dostępne opcje: ",
                new Dictionary<string, Action>
                {
                    { "Składniki",          Ingredients },
                    { "Zamówienia",         Orders },
                    { "Elementy zamówienia", OrderElements },
                    { "Produkty",           Food }
                },
                "Wyjdź");
        }

        private static void Ingredients()
        {
            Menu("Dostępne opcje - składniki: ",
                new Dictionary<string, Action>
                {
                    { "Wszystkie dostępne składniki",   ingredientC.GetAllIngredients },
                    { "Szczegóły składnika",            ingredientC.GetIngredient },
                    { "Dodaj składnik",                 ingredientC.AddIngredient },
                    { "Edytuj składnik",                ingredientC.EditIngredient },
                    { "Usuń składnik",                  ingredientC.DeleteIngredient },
                    { "Filtruj",                        ingredientC.GetFilteredIngredients }
                },
                "Powrót");
        }

        private static void Orders()
        {
            Menu("Dostępne opcje - Zamówienia: ",
                new Dictionary<string, Action>
                {
                    { "Lista wszystkich zamówień",              orderC.GetAllOrders },
                    { "Zawartość zamówienia",                   orderC.GetOrder },
                    { "Oczekujące",                             orderC.GetOrdersWaiting },
                    { "W trakcie",                              orderC.GetOrdersInProgress },
                    { "Gotowe do dostarczenia",                 orderC.GetOrdersReadyForDelivery },
                    { "Dodaj zamówienie",                       orderC.AddOrder },
                    //{ "Zmień zawartość zamówienia",             orderC.EditOrder },
                    { "Usuń zamówienie",                        orderC.DeleteOrder },
                    { "Zmień status zamówienia",                orderC.ChangeOrderStatus },
                    { "Przenieś zamówienia do kolejnego etapu", orderC.MoveToNextStatus },
                    { "Zmień priorytet zamówienia",             orderC.ChangeOrderPriority },
                    { "Policz cenę dla zamówienia",             orderC.CalculatePriceForOrder },
                    { "Wszystkie podelementy zamówienia",       orderC.ShowAllSubOrderElements },
                    { "Filtruj zamówienia",                     orderC.GetFilteredOrders}           
                },
                "Powrót");
        }

        private static void Food()
        {
            Menu("Dostępne opcje - Produkty:",
                new Dictionary<string, Action>
                {
                    { "Lista wszystkich produktów", foodC.GetAllFood },
                    { "Szczegóły produktu", foodC.GetFood },
                    { "Dodaj produkt", foodC.AddFood },
                    { "Usuń produkt", foodC.DeleteFood },
                    { "Szukaj wg filtru", foodC.GetFilteredFood }
                }, "Powrót");
        }

        private static void OrderElements()
        {
            Menu("Dostępne opcje - Elementy zamówienia:",
                new Dictionary<string, Action>
                {
                    { "Lista wszystkich elementów zamówień", orderElemC.GetAllOrderElements },
                    { "Lista elementów zamówienia", orderElemC.GetAllElementsForOrder },
                    { "Dodaj element do zamówienia", orderElemC.AddOrderElement},
                    { "Zmień ilość", orderElemC.ChangeAmount },
                    { "Usuń element", orderElemC.DeleteOrderElement }
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
                Console.WriteLine(new string('-', header.Length));
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

        private static void UserAuthentication()
        {
            // logowanie
            Menu("Zaloguj się lub zarejestruj", new Dictionary<string, Action>
            {
                {"Zaloguj się", userC.Login}, // redirect do głównego menu
                {"Zarejestruj", userC.Register}
            }, "Wyjdź");
            var currentUser = container.Resolve<User>("CurrentUser");
            if (!currentUser.IsLogged) Environment.Exit(0);
            // update rights to correct rights
            user = GetAccountType(currentUser);
        }

        private static BaseRights GetAccountType(User localUser)
        {
            switch (localUser.AccountType)
            {
                case AccountType.ROOT: return new Root();
                case AccountType.DRIVER: return new Driver();
                case AccountType.CASHIER: return new Cashier();
                default: return new BaseRights();
            }
        }
    }
}
