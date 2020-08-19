using System.Collections.Generic;

namespace MarioPizzaOriginal.Account
{
    public class Driver : BaseRights
    {
        public Driver()
        {
            var driverPermissions = new List<string> {
                Rights.Foods.GetFood,
                Rights.Foods.GetAllFood,
                Rights.Orders.GetAllOrders,
                Rights.Orders.GetOrdersWaiting,
                Rights.Orders.GetOrdersInProgress,
                Rights.Orders.GetOrdersReadyForDelivery,
                Rights.Orders.ChangeOrderStatus,
                Rights.Orders.MoveToNextStatus,
                Rights.Orders.CalculatePriceForOrder,
                Rights.OrderElements.GetAllElementsForOrder
            };
            Permissions.AddRange(driverPermissions);
        }
    }
}
