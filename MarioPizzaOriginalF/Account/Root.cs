using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioPizzaOriginal.Account
{
    public class Root : User
    {
        public Root()
        {
            Rights = new List<string> {
            "Ingredients",
                "AddIngredient",
                "GetAllIngredients",
                "EditIngredient",
                "DeleteIngredient",
                "GetIngredient",
            "Food",
                "GetFood",
                "GetAllFood",
                "GetFilteredFood",
            "Orders",
                "GetAllOrders",
                "GetOrder",
                "GetOrdersWaiting",
                "GetOrdersInProgress",
                "GetOrdersReadyForDelivery",
                "AddOrder",
                "EditOrder",
                "DeleteOrder",
                "ChangeOrderStatus",
                "MoveToNextStatus",
                "ChangeOrderPriority",
                "CalculatePriceForOrder",
                "ShowAllSubOrderElements",
            "OrderElements",
                "GetAllOrderElements",
                "GetAllElementsForOrder",
                "AddOrderElement",
                "ChangeAmount",
                "DeleteOrderElement"
        };
        }
    }
}
