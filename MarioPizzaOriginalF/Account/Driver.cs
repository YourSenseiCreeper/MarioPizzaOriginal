using System.Collections.Generic;

namespace MarioPizzaOriginal.Account
{
    public class Driver : User
    {
        public Driver()
        {
            Rights = new List<string> {
            "Food",
                "GetFood",
                "GetAllFood",
            "Orders",
                "GetAllOrders",
                "GetOrder",
                "GetOrdersWaiting",
                "GetOrdersInProgress",
                "GetOrdersReadyForDelivery",
                "ChangeOrderStatus",
                "MoveToNextStatus",
                "CalculatePriceForOrder",
            "OrderElements",
                "GetAllElementsForOrder",
        };
        }
    }
}
