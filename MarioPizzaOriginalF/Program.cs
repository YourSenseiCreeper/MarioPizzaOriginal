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
            new DomainStartup(); // registering repositories to IoC
            TinyIoCContainer.Current.Register(new User{Permissions = new BaseRights()}, "CurrentUser");
            var userC = new UserController();
            var orderC = new OrderController();
            var ingredientC = new IngredientController();
            var foodC = new FoodController();
            var orderElemC = new OrderElementController();

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
