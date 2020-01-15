using System;
using System.Collections.Generic;

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
