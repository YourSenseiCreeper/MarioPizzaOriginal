using System;
using System.Collections.Generic;
using System.Linq;

namespace MarioPizzaOriginal.Account
{
    public static class Rights
    {
        public static readonly IEnumerable<string> All = Ingredients.Global
            .Concat(Foods.Global)
            .Concat(Orders.Global)
            .Concat(OrderElements.Global)
            .Concat(User.Global)
            .Concat(Role.Global);

        public static readonly IEnumerable<string> Basic = User.Basic
            .Concat(new []
            {
                Ingredients.IngredientsMenu,
                Foods.FoodMenu,
                Orders.OrdersMenu,
                OrderElements.OrderElementsMenu
            });

        internal static class Ingredients
        {
            public static readonly string IngredientsMenu = "IngredientsMenu";
            public static readonly string AddIngredient = "AddIngredient";
            public static readonly string GetAllIngredients = "GetAllIngredients";
            public static readonly string EditIngredient = "EditIngredient";
            public static readonly string GetIngredient = "GetIngredient";
            public static readonly string GetFilteredIngredients = "GetFilteredIngredients";
            public static readonly string[] Global = {
                IngredientsMenu, AddIngredient, GetAllIngredients,
                EditIngredient, GetIngredient, GetFilteredIngredients};
        }

        internal static class Foods
        {
            public const string FoodMenu = "FoodMenu";
            public const string GetFood = "GetFood";
            public const string GetAllFood = "GetAllFood";
            public const string AddFood = "AddFood";
            public const string DeleteFood = "DeleteFood";
            public const string GetFilteredFood = "GetFilteredFood";
            public static readonly string[] Global =
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
            public static readonly string[] Global =
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
            public static readonly string[] Global =
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

            public static readonly string[] Basic = { UserMenu, Register, Login };
            public static readonly string[] Global =
            {
                UserMenu, Register, Login, ShowCurrentUserInfo, ResetPassword, ShowAllAccounts,
                ShowAccountInfo, Logout
            };
        }

        internal static class Role
        {
            public const string RoleMenu = "RoleMenu";
            public const string GetAllRoles = "GetAllRoles";
            public const string GetRolePrivileges = "GetRolePrivileges";
            public const string AddNewRole = "AddNewRole";
            public const string ManagePrivileges = "ManagePrivileges";
            public const string SaveCurrentPrivileges = "SaveCurrentPrivileges";
            public static readonly string[] Global =
            {
                RoleMenu, GetAllRoles, GetRolePrivileges, AddNewRole, ManagePrivileges, SaveCurrentPrivileges
            };
        }
    }
}
