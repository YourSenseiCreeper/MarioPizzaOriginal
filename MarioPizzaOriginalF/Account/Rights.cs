using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Account
{
    public static class Rights
    {
        public static class Ingredients
        {
            public static string IngredientsMenu = "Ingredients";
            public static string AddIngredient = "AddIngredient";
            public static string GetAllIngredients = "GetAllIngredients";
            public static string EditIngredient = "EditIngredient";
            public static string GetIngredient = "GetIngredient";
            public static IEnumerable<string> Global = new[] {
                IngredientsMenu, AddIngredient, GetAllIngredients,
                EditIngredient, GetIngredient};
        }
        
        public static class Food
        {
            public static string FoodMenu = "Food";
            public static string GetFood = "GetFood";
            public static string GetAllFood = "GetAllFood";
            public static string AddFood = "AddFood";
            public static string DeleteFood = "DeleteFood";
            public static string GetFilteredFood = "GetFilteredFood";
            public static IEnumerable<string> Global = new[]
            {
                FoodMenu, GetFood, GetAllFood,
                AddFood, DeleteFood, GetFilteredFood
            };
        }

        public static class Orders
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

        public static class OrderElements
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
