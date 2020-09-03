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
            container.Register(new User{Permissions = new BaseRights()}, "CurrentUser");
            var userC = new UserController(container);
            var orderC = new OrderController(container);
            var ingredientC = new IngredientController(container);
            var foodC = new FoodController(container);
            var orderElemC = new OrderElementController(container);

            userC.UserAuthentication();

            MenuCreator.Create()
                .SetHeader("Dostępne opcje: ")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Składniki", ingredientC.IngredientsMenu},
                    {"Zamówienia", orderC.OrdersMenu},
                    {"Elementy zamówienia", orderElemC.OrderElementsMenu},
                    {"Produkty", foodC.FoodMenu},
                    {"Użytkownik", userC.UserMenu}
                })
                .AddFooter("Wyjdź")
            .Present();
        }
    }
}
