using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Account
{
    public class Root : User
    {
        public Root()
        {
            Permissions = new List<string>();
            Permissions.AddRange(Rights.Ingredients.Global);
            Permissions.AddRange(Rights.Food.Global);
            Permissions.AddRange(Rights.Orders.Global);
            Permissions.AddRange(Rights.OrderElements.Global);
        }
    }
}
