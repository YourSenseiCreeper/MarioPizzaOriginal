using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain;

namespace MarioPizzaOriginal.Account
{
    public class BaseRights
    {
        public List<string> Permissions { get; set; }

        public BaseRights()
        {
            Permissions = new List<string>(Rights.User.Basic);
            var other = new List<string>
            {
                Rights.Foods.FoodMenu,
                Rights.Ingredients.IngredientsMenu,
                Rights.OrderElements.OrderElementsMenu,
                Rights.Orders.OrdersMenu,
            };
            Permissions.AddRange(other);
        }

        public bool HasPermission(string methodName) => Permissions.Contains(methodName);

        public static BaseRights GetAccountPermissions(User localUser)
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
