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
        public static void Main(string[] args)
        {
            var container = TinyIoCContainer.Current;
            new DomainStartup(container); // registering repositories to IoC
            container.Register(new BaseRights(), "CurrentUser");
            var userC = new UserController(container);
            var orderC = new OrderController(container);
            var ingredientC = new IngredientController(container);
            var foodC = new FoodController(container);
            var orderElemC = new OrderElementController(container);
            
            userC.UserAuthentication();

            ViewHelper.Menu("Dostępne opcje: ",
                new Dictionary<string, Action>
                {
                    { "Składniki",          ingredientC.IngredientsMenu },
                    { "Zamówienia",         orderC.OrdersMenu },
                    { "Elementy zamówienia", orderElemC.OrderElementsMenu },
                    { "Produkty",           foodC.FoodMenu },
                    { "Użytkownik",         userC.UserMenu }
                },
                "Wyjdź");
        }

        // private static void Ingredients()
        // {
        //     Menu("Dostępne opcje - składniki: ",
        //         new Dictionary<string, Action>
        //         {
        //             { "Wszystkie dostępne składniki",   ingredientC.GetAllIngredients },
        //             { "Szczegóły składnika",            ingredientC.GetIngredient },
        //             { "Dodaj składnik",                 ingredientC.AddIngredient },
        //             { "Edytuj składnik",                ingredientC.EditIngredient },
        //             { "Usuń składnik",                  ingredientC.DeleteIngredient },
        //             { "Filtruj",                        ingredientC.GetFilteredIngredients }
        //         },
        //         "Powrót");
        // }
        //
        // private static void Orders()
        // {
        //     Menu("Dostępne opcje - Zamówienia: ",
        //         new Dictionary<string, Action>
        //         {
        //             { "Lista wszystkich zamówień",              orderC.GetAllOrders },
        //             { "Zawartość zamówienia",                   orderC.GetOrder },
        //             { "Oczekujące",                             orderC.GetOrdersWaiting },
        //             { "W trakcie",                              orderC.GetOrdersInProgress },
        //             { "Gotowe do dostarczenia",                 orderC.GetOrdersReadyForDelivery },
        //             { "Dodaj zamówienie",                       orderC.AddOrder },
        //             //{ "Zmień zawartość zamówienia",             orderC.EditOrder },
        //             { "Usuń zamówienie",                        orderC.DeleteOrder },
        //             { "Zmień status zamówienia",                orderC.ChangeOrderStatus },
        //             { "Przenieś zamówienia do kolejnego etapu", orderC.MoveToNextStatus },
        //             { "Zmień priorytet zamówienia",             orderC.ChangeOrderPriority },
        //             { "Policz cenę dla zamówienia",             orderC.CalculatePriceForOrder },
        //             { "Wszystkie podelementy zamówienia",       orderC.ShowAllSubOrderElements },
        //             { "Filtruj zamówienia",                     orderC.GetFilteredOrders}           
        //         },
        //         "Powrót");
        // }
        //
        // private static void Food()
        // {
        //     Menu("Dostępne opcje - Produkty:",
        //         new Dictionary<string, Action>
        //         {
        //             { "Lista wszystkich produktów", foodC.GetAllFood },
        //             { "Szczegóły produktu", foodC.GetFood },
        //             { "Dodaj produkt", foodC.AddFood },
        //             { "Usuń produkt", foodC.DeleteFood },
        //             { "Szukaj wg filtru", foodC.GetFilteredFood }
        //         }, "Powrót");
        // }
        //
        // private static void OrderElements()
        // {
        //     Menu("Dostępne opcje - Elementy zamówienia:",
        //         new Dictionary<string, Action>
        //         {
        //             { "Lista wszystkich elementów zamówień", orderElemC.GetAllOrderElements },
        //             { "Lista elementów zamówienia", orderElemC.GetAllElementsForOrder },
        //             { "Dodaj element do zamówienia", orderElemC.AddOrderElement},
        //             { "Zmień ilość", orderElemC.ChangeAmount },
        //             { "Usuń element", orderElemC.DeleteOrderElement }
        //         }, "Powrót");
        // }
    }
}
