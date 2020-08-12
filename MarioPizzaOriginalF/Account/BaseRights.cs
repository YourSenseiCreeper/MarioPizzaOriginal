using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Rights.Orders.OrdersMenu
            };
            Permissions.AddRange(other);
        }
    }
}
