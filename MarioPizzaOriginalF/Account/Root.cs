using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Account
{
    public class Root : BaseRights
    {
        public Root()
        {
            Permissions.AddRange(Rights.Ingredients.Global);
            Permissions.AddRange(Rights.Foods.Global);
            Permissions.AddRange(Rights.Orders.Global);
            Permissions.AddRange(Rights.OrderElements.Global);
            Permissions.AddRange(Rights.User.Global);
        }
    }
}
