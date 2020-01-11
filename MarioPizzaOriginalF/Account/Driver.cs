using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
