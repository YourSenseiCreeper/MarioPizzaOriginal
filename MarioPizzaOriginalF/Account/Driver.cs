using System.Collections.Generic;

namespace MarioPizzaOriginal.Account
{
    public class Driver : User
    {
        public Driver()
        {
            Permissions = new List<string> {
                Rights.Food.FoodMenu,
                Rights.Food.GetFood,
                Rights.Food.GetAllFood,
                Rights.Orders.OrdersMenu,
                Rights.Orders.GetAllOrders,
                Rights.Orders.GetOrdersWaiting,
                Rights.Orders.GetOrdersInProgress,
                Rights.Orders.GetOrdersReadyForDelivery,
                Rights.Orders.ChangeOrderStatus,
                Rights.Orders.MoveToNextStatus,
                Rights.Orders.CalculatePriceForOrder,
                Rights.OrderElements.OrderElementsMenu,
                Rights.OrderElements.GetAllElementsForOrder
            };
        }
    }
}
