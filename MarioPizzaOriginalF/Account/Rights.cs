using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Account
{
    public static class Rights
    {
        internal static class Ingredients
        {
            public static readonly string IngredientsMenu = "IngredientsMenu";
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
            public static readonly string FoodMenu = "FoodMenu";
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
            public const string OrdersMenu = "OrdersMenu";
            public const string GetAllOrders = "GetAllOrders";
            public const string GetOrder = "GetOrder";
            public const string GetOrdersWaiting = "GetOrdersWaiting";
            public const string GetOrdersInProgress = "GetOrdersInProgress";
            public const string GetOrdersReadyForDelivery = "GetOrdersReadyForDelivery";
            public const string AddOrder = "AddOrder";
            public const string EditOrder = "EditOrder";
            public const string DeleteOrder = "DeleteOrder";
            public const string ChangeOrderStatus = "ChangeOrderStatus";
            public const string MoveToNextStatus = "MoveToNextStatus";
            public const string ChangePriority = "ChangeOrderPriority";
            public const string CalculatePriceForOrder = "CalculatePriceForOrder";
            public const string ShowAllSubOrderElements = "ShowAllSubOrderElements";
            public const string GetFilteredOrders = "GetFilteredOrders";
            public const string AddOrderElement = "AddOrderElement";
            public static readonly IEnumerable<string> Global = new[]
            {
                OrdersMenu, GetAllOrders, GetOrder, GetOrdersWaiting, GetOrdersInProgress,
                GetOrdersReadyForDelivery, AddOrder, EditOrder, DeleteOrder,
                ChangeOrderStatus, MoveToNextStatus, ChangePriority,
                CalculatePriceForOrder, ShowAllSubOrderElements, GetFilteredOrders, AddOrderElement
            };
        }

        internal static class OrderElements
        {
            public const string OrderElementsMenu = "OrderElementsMenu";
            public const string GetAllOrderElements = "GetAllOrderElements";
            public const string GetAllElementsForOrder = "GetAllElementsForOrder";
            public const string AddOrderElement = "AddOrderElement";
            public const string ChangeAmount = "ChangeAmount";
            public const string DeleteOrderElement = "DeleteOrderElement";
            public static readonly IEnumerable<string> Global = new[]
            {
                OrderElementsMenu, GetAllOrderElements, GetAllElementsForOrder,
                AddOrderElement, ChangeAmount, DeleteOrderElement
            };
        }

        internal static class User
        {
            public const string UserMenu = "UserMenu";
            public const string Register = "Register";
            public const string Login = "Login";
            public const string Logout = "Logout";
            public const string ShowCurrentUserInfo = "ShowCurrentUserInfo";
            public const string ResetPassword = "ResetPassword";
            public const string ShowAllAccounts = "ShowAllAccounts";
            public const string ShowAccountInfo = "ShowAccountInfo";

            public static readonly IEnumerable<string> Basic = new[]
            {
                UserMenu, Register, Login
            };
            public static readonly IEnumerable<string> Global = new[]
            {
                UserMenu, Register, Login, ShowCurrentUserInfo, ResetPassword, ShowAllAccounts,
                ShowAccountInfo, Logout
            };
        }
    }
}
