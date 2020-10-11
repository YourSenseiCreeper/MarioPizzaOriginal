using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Account;
using MarioPizzaOriginal.Controller;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Tools;
using TinyIoC;

namespace MarioPizzaOriginal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var current = TinyIoCContainer.Current;
            new DomainStartup(); // registering repositories for IoC
            current.Register<IConsole>(new PizzaConsole());
            var userC = new UserController();
            var orderC = new OrderController();
            var ingredientC = new IngredientController();
            var foodC = new FoodController();
            var orderElemC = new OrderElementController();
            var roleC = new RoleController();

            var roleRepository = current.Resolve<IRoleRepository>();
            roleRepository.UpdateDefaultRoles();

            current.Register(new User { Role =  roleRepository.Get(r => r.Name == "BASIC")}, "CurrentUser");

            userC.UserAuthentication();

            MenuCreator.Create()
                .SetHeader("Dostępne opcje: ")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Składniki", ingredientC.IngredientsMenu},
                    {"Zamówienia", orderC.OrdersMenu},
                    {"Elementy zamówienia", orderElemC.OrderElementsMenu},
                    {"Produkty", foodC.FoodMenu},
                    {"Użytkownik", userC.UserMenu},
                    {"Role", roleC.RoleMenu}
                })
                .AddFooter("Wyjdź")
            .Present();
        }
    }
}
