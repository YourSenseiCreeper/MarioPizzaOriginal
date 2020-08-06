using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Account
{
    public static class Rights
    {
        internal static class Ingredients
        {
            public static readonly string IngredientsMenu = "Ingredients";
            public static readonly string AddIngredient = "AddIngredient";
            public static readonly string GetAllIngredients = "GetAllIngredients";
            public static readonly string EditIngredient = "EditIngredient";
            public static readonly string GetIngredient = "GetIngredient";
            public static readonly string GetFilteredIngredients = "GetFilteredIngredients";
            public static readonly IEnumerable<string> Global = new[] {
                IngredientsMenu, AddIngredient, GetAllIngredients,
                EditIngredient, GetIngredient, GetFilteredIngredients};
        }

        internal class Foods
        {
            public static readonly string FoodMenu = "Food";
            public static readonly string GetFood = "GetFood";
            public static readonly string GetAllFood = "GetAllFood";
            public static readonly string AddFood = "AddFood";
            public static readonly string DeleteFood = "DeleteFood";
            public static readonly string GetFilteredFood = "GetFilteredFood";
            public static readonly IEnumerable<string> Global = new[]
            {
                FoodMenu, GetFood, GetAllFood,
                AddFood, DeleteFood, GetFilteredFood
            };
        }

        internal static class Orders
        {
            public static string OrdersMenu = "Orders";
            public static string GetAllOrders = "GetAllOrders";
            public static string GetOrder = "GetOrder";
            public static string GetOrdersWaiting = "GetOrdersWaiting";
            public static string GetOrdersInProgress = "GetOrdersInProgress";
            public static string GetOrdersReadyForDelivery = "GetOrdersReadyForDelivery";
            public static string AddOrder = "AddOrder";
            public static string EditOrder = "EditOrder";
            public static string DeleteOrder = "DeleteOrder";
            public static string ChangeOrderStatus = "ChangeOrderStatus";
            public static string MoveToNextStatus = "MoveToNextStatus";
            public static string ChangePriority = "ChangeOrderPriority";
            public static string CalculatePriceForOrder = "CalculatePriceForOrder";
            public static string ShowAllSubOrderElements = "ShowAllSubOrderElements";
            public static IEnumerable<string> Global = new[]
            {
                OrdersMenu, GetAllOrders, GetOrder, GetOrdersWaiting, GetOrdersInProgress,
                GetOrdersReadyForDelivery, AddOrder, EditOrder, DeleteOrder,
                ChangeOrderStatus, MoveToNextStatus, ChangePriority,
                CalculatePriceForOrder, ShowAllSubOrderElements
            };
        }

        internal static class OrderElements
        {
            public static string OrderElementsMenu = "OrderElements";
            public static string GetAllOrderElements = "GetAllOrderElements";
            public static string GetAllElementsForOrder = "GetAllElementsForOrder";
            public static string AddOrderElement = "AddOrderElement";
            public static string ChangeAmount = "ChangeAmount";
            public static string DeleteOrderElement = "DeleteOrderElement";
            public static IEnumerable<string> Global = new[]
            {
                OrderElementsMenu, GetAllOrderElements, GetAllElementsForOrder,
                AddOrderElement, ChangeAmount, DeleteOrderElement
            };
        }
    }
}
