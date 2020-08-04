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
            Permissions = new List<string> {
                Rights.Food.FoodMenu,
                Rights.Food.GetFood,
                Rights.Food.GetAllFood,
                Rights.Food.GetFilteredFood,
                Rights.Orders.OrdersMenu,
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
                Rights.OrderElements.OrderElementsMenu,
                Rights.OrderElements.GetAllOrderElements,
                Rights.OrderElements.GetAllElementsForOrder,
                Rights.OrderElements.AddOrderElement,
                Rights.OrderElements.ChangeAmount,
                Rights.OrderElements.DeleteOrderElement
            };
        }
    }
}
