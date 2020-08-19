using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioPizzaOriginal.Account
{
    public class Cashier : BaseRights
    {
        public Cashier()
        {
            var cashierPermissions = new List<string> {
                Rights.Foods.GetFood,
                Rights.Foods.GetAllFood,
                Rights.Foods.GetFilteredFood,
                Rights.Orders.GetAllOrders,
                Rights.Orders.GetOrder,
                Rights.Orders.GetOrdersWaiting,
                Rights.Orders.GetOrdersInProgress,
                Rights.Orders.GetOrdersReadyForDelivery,
                Rights.Orders.AddOrder,
                Rights.Orders.EditOrder,
                Rights.Orders.DeleteOrder,
                Rights.Orders.ChangeOrderStatus,
                Rights.Orders.MoveToNextStatus,
                Rights.Orders.ChangeOrderStatus,
                Rights.Orders.CalculatePriceForOrder,
                Rights.OrderElements.GetAllOrderElements,
                Rights.OrderElements.GetAllElementsForOrder,
                Rights.OrderElements.AddOrderElement,
                Rights.OrderElements.ChangeAmount,
                Rights.OrderElements.DeleteOrderElement
            };
            Permissions.AddRange(cashierPermissions);
        }
    }
}
