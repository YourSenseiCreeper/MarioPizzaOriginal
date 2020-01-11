using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioPizzaOriginal.Account
{
    public class Cashier : User
    {
        public Cashier()
        {
            Rights = new List<string> {
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
